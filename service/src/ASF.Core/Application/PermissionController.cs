using ASF.Application.DTO;
using ASF.Domain;
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
    /// 功能权限
    /// </summary>
    [Authorize]
    [Route("[controller]/[action]")]
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

        #region 功能权限
        /// <summary>
        /// 创建功能权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> CreateAction([FromBody]PermissionActionCreateRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //创建功能权限
            var permission = dto.To();
            result = await this._serviceProvider.GetRequiredService<PermissionCreateService>().CreateAction(permission);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionCreateAction, dto, "Success");  //记录日志
            await _permissionRepository.AddAsync(permission);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改功能权限
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

            //修改功能权限
            var modifyResult = await this._serviceProvider.GetRequiredService<PermissionChangeService>()
                .ModifyAction(dto.Id, dto.Name, dto.Description, dto.Enable, dto.ApiTemplate, dto.IsLogger, dto.Sort);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionModifyAction, dto, "Success");  //记录日志
            await _permissionRepository.ModifyAsync(modifyResult.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 获取功能权限集合
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultList<PermissionActionInfoDetailsResponseDto>> GetActionList([FromBody]PermissionListRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return ResultList<PermissionActionInfoDetailsResponseDto>.ReFailure(result);

            //获取权限数据
            var permissionList = await this._permissionRepository.GetList(dto);

            //组合响应数据
            var actionList = permissionList.Where(f => f.Type == PermissionType.Action).OrderBy(f => f.Sort).ToList();
            var actions = Mapper.Map<List<PermissionActionInfoDetailsResponseDto>>(actionList);
            return ResultList<PermissionActionInfoDetailsResponseDto>.ReSuccess(actions);

        }
        /// <summary>
        /// 根据ID获取功能权限详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result<PermissionActionInfoDetailsResponseDto>> GetActionDetails([FromRoute]string id)
        {
            // 获取权限数据
            var action = await this._permissionRepository.GetAsync(id);
            if (action == null || action.Type != PermissionType.Action)
                return Result<PermissionActionInfoDetailsResponseDto>.ReFailure(ResultCodes.PermissionNotExist);

            var result = Mapper.Map<PermissionActionInfoDetailsResponseDto>(action);
            return Result<PermissionActionInfoDetailsResponseDto>.ReSuccess(result);
        }
        #endregion

        #region 菜单权限
        /// <summary>
        /// 创建菜单权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> CreateMenu([FromBody]PermissionMenuCreateRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //创建菜单权限
            var permission = dto.To();
            result = await this._serviceProvider.GetRequiredService<PermissionCreateService>().CreateMenu(permission);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionCreateMenu, dto, "Success");  //记录日志
            await _permissionRepository.AddAsync(permission);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改菜单权限
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

            //修改功能权限
            var modifyResult = await this._serviceProvider.GetRequiredService<PermissionChangeService>().ModifyMenu(dto.Id, dto.Name, dto.ParentId, dto.Description, dto.Enable, dto.Sort);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionModifyMenu, dto, "Success");  //记录日志
            await _permissionRepository.ModifyAsync(modifyResult.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }

        /// <summary>
        /// 获取菜单权限集合
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
            var menuList = permissionList.Where(f => f.Type == PermissionType.Menu).OrderBy(f => f.Sort).ToList();
            var menus = Mapper.Map<List<PermissionMenuInfoDetailsResponseDto>>(menuList);
            //筛选菜单对应的功能权限
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
        /// 获取所有菜单权限集合
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
            //筛选菜单对应的功能权限
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
        /// 根据ID获取菜单权限详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result<PermissionMenuInfoDetailsResponseDto>> GetMenuDetails([FromRoute]string id)
        {
            //获取菜单权限数据
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
        #endregion

        #region 开放性API
        /// <summary>
        /// 创建开放API权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> CreateOpenApi([FromBody]PermissionOpenApiCreateRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //创建菜单权限
            var permission = dto.To();
            result = this._serviceProvider.GetRequiredService<PermissionCreateService>().CreateOpenApi(permission);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionCreateOpenApi, dto, "Success");  //记录日志
            await _permissionRepository.AddAsync(permission);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改开放API权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> ModifyOpenApi([FromBody]PermissionOpenApiModifyRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //修改功能权限
            var modifyResult = await this._serviceProvider.GetRequiredService<PermissionChangeService>().ModifyOpenApi(dto.Id, dto.Name, dto.Description, dto.Enable,dto.ApiTemplate);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionModifyOpenApi, dto, "Success");  //记录日志
            await _permissionRepository.ModifyAsync(modifyResult.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 根据ID获取开放API权限详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result<PermissionOpenApiInfoDetailsResponseDto>> GetOpenApiDetails([FromRoute]string id)
        {
            //获取菜单权限数据
            var menu = await this._permissionRepository.GetAsync(id);
            if (menu == null || menu.Type != PermissionType.OpenApi)
                return Result<PermissionOpenApiInfoDetailsResponseDto>.ReFailure(ResultCodes.PermissionNotExist);
  
            //组装响应对象
            var result = Mapper.Map<PermissionOpenApiInfoDetailsResponseDto>(menu);
            return Result<PermissionOpenApiInfoDetailsResponseDto>.ReSuccess(result);
        }
        /// <summary>
        /// 获取开放API权限集合
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultList<PermissionOpenApiInfoDetailsResponseDto>> GetOpenApiList([FromBody]PermissionListRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return ResultList<PermissionOpenApiInfoDetailsResponseDto>.ReFailure(result);

            //获取权限数据
            var permissionList = await this._permissionRepository.GetList(dto);

            //筛选所有的菜单权限
            var menuList = permissionList.Where(f => f.Type == PermissionType.OpenApi).OrderBy(f => f.Sort).ToList();
            var menus = Mapper.Map<List<PermissionOpenApiInfoDetailsResponseDto>>(menuList);
            return ResultList<PermissionOpenApiInfoDetailsResponseDto>.ReSuccess(menus);
        }
        #endregion

        /// <summary>
        /// 修改权限排序
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> ModifySort([FromBody]PermissionModifySortRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;


            //修改功能权限
            var modifyResult = await this._serviceProvider.GetRequiredService<PermissionChangeService>().ModifySort(dto.Id, dto.Sort);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            await _permissionRepository.ModifyAsync(modifyResult.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id">权限标识</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<Result> Delete([FromRoute]string id)
        {
            //删除权限
            var result = await this._serviceProvider.GetRequiredService<PermissionDeleteService>().Delete(id);
            if (!result.Success)
                return result;

            _operateLog.Record(ASFPermissions.PermissionDelete, new { permissionId = id }, "Success");  //记录日志
            await _permissionRepository.RemoveAsync(id);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
    }
}
