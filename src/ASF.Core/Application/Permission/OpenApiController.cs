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
    /// 开放性API权限
    /// </summary>
    [Authorize]
    [Route("Permission/[controller]/[action]")]
    public class OpenApiController : PermissionController
    {
        public OpenApiController(IServiceProvider serviceProvider, LogOperateRecordService operateLog, IUnitOfWork unitOfWork, IPermissionRepository permissionRepository)
            : base(serviceProvider, operateLog, unitOfWork, permissionRepository)
        {

        }

        /// <summary>
        /// 创建开放API权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create([FromBody]PermissionOpenApiCreateRequestDto dto)
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
        public async Task<Result> Modify([FromBody]PermissionOpenApiModifyRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //修改功能权限
            var modifyResult = await this._serviceProvider.GetRequiredService<PermissionChangeService>().ModifyOpenApi(dto.Id, dto.Name, dto.Description, dto.Enable, dto.ApiTemplate);
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
        public async Task<Result<PermissionOpenApiInfoDetailsResponseDto>> GetDetails([FromRoute]string id)
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
        public async Task<ResultList<PermissionOpenApiInfoDetailsResponseDto>> GetList([FromBody]PermissionListRequestDto dto)
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

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Import([FromBody] PermissionOpenApiRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            if (dto.List != null && dto.List.Count > 0)
            {
                foreach (var item in dto.List)
                {
                    var model = await this._permissionRepository.GetAsync(item.Id);
                    if (model != null)
                    {
                        //修改
                        var entity = item.To();
                        await _permissionRepository.ModifyAsync(entity);
                        await _unitOfWork.CommitAsync(autoRollback: true);
                    }
                    else
                    {
                        //添加
                        var entity = item.To();
                        await _permissionRepository.AddAsync(entity);
                        await _unitOfWork.CommitAsync(autoRollback: true);
                    }
                }
            }
            return Result.ReSuccess();
        }

    }
}
