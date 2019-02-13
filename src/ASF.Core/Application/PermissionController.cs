using ASF.Application.DTO;
using ASF.Domain.Entities;
using ASF.Domain.Services;
using ASF.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
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
        public async Task<Result> CreateAction(PermissionActionCreateRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //创建操作权限
            var permission = Mapper.Map<Permission>(dto);
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
        public async Task<Result> CreateMenu(PermissionMenuCreateRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //创建导航权限
            var permission = Mapper.Map<Permission>(dto);
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
        public async Task<Result> ModifyAction(PermissionActionModifyRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //修改操作权限
            var modifyResult = await this._serviceProvider.GetRequiredService<PermissionChangeService>().ModifyAction(dto.Id, dto.Name, dto.ParentId, dto.Description, dto.Enable, dto.ApiAddress, dto.IsLogger);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionModifyAction, dto.ToString(), "Success");  //记录日志
            await _permissionRepository.AddAsync(modifyResult.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改导航权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Result> ModifyMenu(PermissionMenuModifyRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //修改操作权限
            var modifyResult = await this._serviceProvider.GetRequiredService<PermissionChangeService>().ModifyMenu(dto.Id, dto.Name, dto.ParentId, dto.Description, dto.Enable, dto.Sort);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionModifyMenu, dto.ToString(), "Success");  //记录日志
            await _permissionRepository.AddAsync(modifyResult.Data);
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
    }
}
