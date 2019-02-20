using ASF.Application.DTO;
using ASF.Domain;
using ASF.Domain.Services;
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
        /// 获取所有的角色基本信息
        /// </summary>
        /// <returns></returns>
     
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
            int createOfAccountId = HttpContext.User.UserId();
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
        /// 获取所有角色基本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "self")]
        public async Task<ResultList<RoleInfoSimpleResponseDto>> GetListAll()
        {
            //获取角色数据
            var roelList = await this._roleRepository.GetList();
            if (roelList.Count == 0)
                return ResultList<RoleInfoSimpleResponseDto>.ReSuccess();

            var roles = Mapper.Map<IList<RoleInfoSimpleResponseDto>>(roelList);
            return ResultList<RoleInfoSimpleResponseDto>.ReSuccess(roles);
        }
        /// <summary>
        /// 获取角色集合
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultPagedList<RoleInfoBaseResponseDto>> GetList([FromBody]RoleListPagedRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return ResultPagedList<RoleInfoBaseResponseDto>.ReFailure(result);

            //获取角色
            var roelResult = await this._roleRepository.GetList(dto);
            var roles = Mapper.Map<IList<RoleInfoBaseResponseDto>>(roelResult.Roles);
            return ResultPagedList<RoleInfoBaseResponseDto>.ReSuccess(roles, roelResult.TotalCount);
        }
        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result<RoleInfoDetailsResponseDto>> GetDetails([FromRoute]int id)
        {
            var role = await this._roleRepository.GetAsync(id);
            if (role == null)
                return Result<RoleInfoDetailsResponseDto>.ReFailure(ResultCodes.RoleNotExist);

            //获取所有权限
            var permissions = await this._serviceProvider.GetRequiredService<IPermissionRepository>()
                .GetList(role.Permissions.ToList());

            var result = Mapper.Map<RoleInfoDetailsResponseDto>(role);
            result.Permissions = permissions.ToDictionary(k => k.Id, v => v.Name);
            return Result<RoleInfoDetailsResponseDto>.ReSuccess(result);
        }
    }
}
