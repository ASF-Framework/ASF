using ASF.Application.DTO;
using ASF.Domain.Services;
using ASF.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ASF.Application
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository;
        private readonly LogOperateRecordService _operateLog;
        public RoleController(IServiceProvider serviceProvider, IUnitOfWork unitOfWork, IRoleRepository roleRepository, LogOperateRecordService operateLog)
        {
            this._serviceProvider = serviceProvider;
            this._unitOfWork = unitOfWork;
            this._roleRepository = roleRepository;
            this._operateLog = operateLog;
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create([FromBody]RoleCreateRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //调用领域服务
            int createOfAccountId = Convert.ToInt32(HttpContext.User.FindFirst("sub"));
            var createResult = await this._serviceProvider.GetRequiredService<RoleCreateService>()
                .Create(dto.Name, dto.Description, createOfAccountId, dto.Permissions);
            if (!createResult.Success)
                return createResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.RoleCreate, dto.ToString(), "Success");  //记录日志
            await _roleRepository.AddAsync(createResult.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色标识</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Delete([FromRoute]int id)
        {
            _operateLog.Record(ASFPermissions.RoleDelete, id.ToString(), "Success");  //记录日志
            await this._roleRepository.RemoveAsync(id);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Modify([FromBody] RoleModifyRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //调用领域服务修改
            var modifyResult = this._serviceProvider.GetRequiredService<RoleInfoChangeService>()
                    .Modify(dto.RoleId, dto.Name, dto.Enable, dto.Description);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.RoleModify, dto.ToString(), "Success");  //记录日志
            await _roleRepository.ModifyAsync(modifyResult.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改角色状态
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Result> ModifyStatus([FromBody] RoleModifyStatusRequestDto dto)
        {
            if (dto.RoleId <= 0)
                return Result.ReFailure(Domain.ResultCodes.RoleNotExist);

            //数据持久化
            _operateLog.Record(ASFPermissions.RoleModifyStatus, dto.ToString(), "Success");  //记录日志
            await _roleRepository.ModifyAsync(dto.RoleId, dto.Enable);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 获取角色集合
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultPagedList<RoleInfoResponseDto>> List([FromBody]RoleInfoListPagedRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return ResultPagedList<RoleInfoResponseDto>.ReFailure(result);

            //获取角色
            var roelResult = await this._roleRepository.GetList(dto);
            var roles = Mapper.Map<IList<RoleInfoResponseDto>>(roelResult.Roles);
            return ResultPagedList<RoleInfoResponseDto>.ReSuccess(roles, roelResult.TotalCount);
        }

    }
}
