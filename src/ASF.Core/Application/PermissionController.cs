using ASF.Application.DTO;
using ASF.Domain;
using ASF.Domain.Entities;
using ASF.Domain.Services;
using ASF.Domain.Values;
using ASF.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASF.Application
{
    /// <summary>
    /// 操作权限
    /// </summary>
    [Authorize]
    public class PermissionController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly LogOperateRecordService _operateLog;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPermissionRepository _permissionRepository;

        public PermissionController(IServiceProvider serviceProvider, LogOperateRecordService operateLog, IUnitOfWork unitOfWork, IPermissionRepository permissionRepository)
        {
            _serviceProvider = serviceProvider;
            _operateLog = operateLog;
            _unitOfWork = unitOfWork;
            _permissionRepository = permissionRepository;
        }

        /// <summary>
        /// 创建操作权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> CreateAction([FromBody]PermissionActionCreateRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //创建操作权限
            var permission = dto.To();
            result = await this._serviceProvider.GetRequiredService<PermissionCreateService>().CreateAction(permission);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionCreateAction, dto.ToString(), "Success");  //记录日志
            await _permissionRepository.AddAsync(permission);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 创建导航权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> CreateMenu([FromBody]PermissionMenuCreateRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //创建导航权限
            var permission = dto.To();
            result = await this._serviceProvider.GetRequiredService<PermissionCreateService>().CreateMenu(permission);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionCreateMenu, dto.ToString(), "Success");  //记录日志
            await _permissionRepository.AddAsync(permission);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改操作权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> ModifyAction([FromBody]PermissionActionModifyRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //修改操作权限
            var modifyResult = await this._serviceProvider.GetRequiredService<PermissionChangeService>()
                .ModifyAction(dto.Id, dto.Name, dto.ParentId, dto.Description, dto.Enable, dto.ApiTemplate, dto.IsLogger,dto.Sort);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionModifyAction, dto.ToString(), "Success");  //记录日志
            await _permissionRepository.ModifyAsync(modifyResult.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改导航权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> ModifyMenu([FromBody]PermissionMenuModifyRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //修改操作权限
            var modifyResult = await this._serviceProvider.GetRequiredService<PermissionChangeService>().ModifyMenu(dto.Id, dto.Name, dto.ParentId, dto.Description, dto.Enable, dto.Sort);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionModifyMenu, dto.ToString(), "Success");  //记录日志
            await _permissionRepository.ModifyAsync(modifyResult.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id">权限标识</param>
        /// <returns></returns>
        public async Task<Result> Delete([FromRoute]string id)
        {
            //删除权限
            var result = await this._serviceProvider.GetRequiredService<PermissionDeleteService>().Delete(id);
            if (!result.Success)
                return result;

            _operateLog.Record(ASFPermissions.PermissionDelete, id.ToString(), "Success");  //记录日志
            await _permissionRepository.RemoveAsync(id);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 获取导航权限集合
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultList<PermissionMenuInfoDetailsResponseDto>> GetMenuList([FromBody]PermissionListRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return ResultList<PermissionMenuInfoDetailsResponseDto>.ReFailure(result);

            //获取权限数据
            var permissionList = await this._permissionRepository.GetList(dto);

            //筛选所有的菜单权限
            var menuList = permissionList.Where(f=>f.Type == PermissionType.Menu).OrderBy(f => f.Sort).ToList();
            var menus = Mapper.Map<List<PermissionMenuInfoDetailsResponseDto>>(menuList);
            //筛选菜单对应的操作权限
            menus.ForEach(m =>
            {
                m.Actions = permissionList
                     .Where(f => f.Type == PermissionType.Action && f.ParentId == m.Id)
                     .OrderBy(f => f.Sort).ToList()
                     .ToDictionary(k => k.Id, v => v.Name);
            });
            return ResultList<PermissionMenuInfoDetailsResponseDto>.ReSuccess(menus);
        }
        /// <summary>
        /// 获取所有导航权限集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "self")]
        public async Task<ResultList<PermissionMenuInfoBaseResponseDto>> GetMenuAllList()
        {
            //获取权限数据
            var permissionList = await this._permissionRepository.GetList();

            //筛选所有的菜单权限
            var menuList = permissionList.Where(p => p.Type == PermissionType.Menu).OrderBy(f => f.Sort).ToList();
            var menus = Mapper.Map<List<PermissionMenuInfoBaseResponseDto>>(menuList);
            //筛选菜单对应的操作权限
            menus.ForEach(m =>
            {
                m.Actions = permissionList
                     .Where(f => f.Type == PermissionType.Action && f.ParentId == m.Id)
                     .OrderBy(f => f.Sort).ToList()
                     .ToDictionary(k => k.Id, v => v.Name);
            });
            return ResultList<PermissionMenuInfoBaseResponseDto>.ReSuccess(menus);
        }
        /// <summary>
        /// 根据ID获取导航权限详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result<PermissionMenuInfoDetailsResponseDto>> GetMenuDetails([FromRoute]string id)
        {
            //获取导航权限数据
            var menu = await this._permissionRepository.GetAsync(id);
            if (menu == null || menu.Type != PermissionType.Menu)
                return Result<PermissionMenuInfoDetailsResponseDto>.ReFailure(ResultCodes.PermissionNotExist);
            //获取子级权限
            var permissions = await this._permissionRepository.GetListByParentId(menu.Id);

            //组装响应对象
            var result = Mapper.Map<PermissionMenuInfoDetailsResponseDto>(menu);
            result.Actions = permissions
                     .Where(f => f.Type == PermissionType.Action)
                     .ToList()
                     .ToDictionary(k => k.Id, v => v.Name);
            return Result<PermissionMenuInfoDetailsResponseDto>.ReSuccess(result);
        }
        /// <summary>
        /// 获取操作权限集合
        /// </summary>
        /// <returns></returns>
        public async Task<ResultList<PermissionActionInfoDetailsResponseDto>> GetActionList([FromBody]PermissionListRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return ResultList<PermissionActionInfoDetailsResponseDto>.ReFailure(result);

            //获取权限数据
            var permissionList = await this._permissionRepository.GetList(dto);

            //组合响应数据
            var actionList = permissionList.Where(f=>f.Type== PermissionType.Action).OrderBy(f => f.Sort).ToList();
            var actions = Mapper.Map<List<PermissionActionInfoDetailsResponseDto>>(actionList);
            return ResultList<PermissionActionInfoDetailsResponseDto>.ReSuccess(actions);

        }
        /// <summary>
        /// 根据ID获取操作权限详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<PermissionActionInfoDetailsResponseDto>> GetActionDetails([FromRoute]string id)
        {
            // 获取权限数据
            var action = await this._permissionRepository.GetAsync(id);
            if (action == null || action.Type != PermissionType.Action)
                return Result<PermissionActionInfoDetailsResponseDto>.ReFailure(ResultCodes.PermissionNotExist);

            var result = Mapper.Map<PermissionActionInfoDetailsResponseDto>(action);
            return Result<PermissionActionInfoDetailsResponseDto>.ReSuccess(result);
        }
    }
}
