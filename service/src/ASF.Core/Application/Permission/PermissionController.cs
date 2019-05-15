using ASF.Application.DTO;
using ASF.Domain.Services;
using ASF.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ASF.Application
{
    public  class PermissionController : Controller
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly LogOperateRecordService _operateLog;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IPermissionRepository _permissionRepository;

        public PermissionController(IServiceProvider serviceProvider, LogOperateRecordService operateLog, IUnitOfWork unitOfWork, IPermissionRepository permissionRepository)
        {
            _serviceProvider = serviceProvider;
            _operateLog = operateLog;
            _unitOfWork = unitOfWork;
            _permissionRepository = permissionRepository;
        }
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
