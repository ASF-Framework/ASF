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
using System.Net.Http;
using System.Threading.Tasks;

namespace ASF.Application
{
    /// <summary>
    /// 功能权限
    /// </summary>
    [Authorize]
    [Route("Permission/[controller]/[action]")]
    public class ActionController : PermissionController
    {
        public ActionController(IServiceProvider serviceProvider, LogOperateRecordService operateLog, IUnitOfWork unitOfWork, IPermissionRepository permissionRepository)
            :base(serviceProvider, operateLog, unitOfWork, permissionRepository)
        {
          
        }


        /// <summary>
        /// 创建功能权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create([FromBody]PermissionActionCreateRequestDto dto)
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
        public async Task<Result> Modify([FromBody]PermissionActionModifyRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //修改功能权限
            var modifyResult = await this._serviceProvider.GetRequiredService<PermissionChangeService>()
                .ModifyAction(dto.Id, dto.Name, dto.Description, dto.Enable, dto.ApiTemplate, dto.IsLogger, dto.HttpMethods.Select(f => new HttpMethod(f)).ToList());
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
        public async Task<ResultList<PermissionActionInfoDetailsResponseDto>> GetList([FromBody]PermissionListRequestDto dto)
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
        public async Task<Result<PermissionActionInfoDetailsResponseDto>> GetDetails([FromRoute]string id)
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
