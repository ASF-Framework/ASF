using ASF.Application.DTO;
using ASF.Domain.Entities;
using ASF.Domain.Services;
using ASF.Domain.Values;
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
    /// 账户控制器
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;
        private readonly LogOperateRecordService _operateLog;
        public AccountController(IServiceProvider serviceProvider, IUnitOfWork unitOfWork, IAccountRepository accountRepository, LogOperateRecordService operateLog)
        {
            this._serviceProvider = serviceProvider;
            this._unitOfWork = unitOfWork;
            this._accountRepository = accountRepository;
            this._operateLog = operateLog;
        }
        /// <summary>
        /// 创建账号
        /// </summary>
        /// <param name="dto">账号对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create([FromBody]AccountCreateRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //创建服务
            var account = Mapper.Map<Account>(dto);
            var service = this._serviceProvider.GetRequiredService<AccountCreateService>();
            result = await service.Create(account);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.AccountCreate, dto.ToString(), "Success");  //记录日志
            await _accountRepository.AddAsync(account);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 删除账号
        /// </summary>
        /// <param name="id">账号标识</param>
        /// <returns></returns>
        public async Task<Result> Delete([FromRoute]int id)
        {
            _operateLog.Record(ASFPermissions.AccountDelete, id.ToString(), "Success");  //记录日志
            await _accountRepository.RemoveAsync(id);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改账号状态
        /// </summary>
        /// <param name="dto">修改状态请求</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> MidifyStatus([FromBody]AccountModifyStatusRequestDto dto)
        {
            var service = this._serviceProvider.GetRequiredService<AccountInfoChangeService>();
            var result = await service.ModifyStatus(dto.Id, dto.Status);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.AccountModifyInfo, dto.ToString(), "Success");  //记录日志
            await _accountRepository.ModifyAsync(result.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 设置登录密码
        /// </summary>
        /// <param name="dto">设置登录密码</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> ResetPassword([FromBody] AccountResetPasswordRequestDto dto)
        {
            var service = this._serviceProvider.GetRequiredService<AccountPasswordChangeService>();
            var result = await service.ModifyAsync(dto.Id, dto.Password);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.AccountSetPassword, dto.ToString(), "Success");  //记录日志
            await _accountRepository.ModifyAsync(result.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }

        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <param name="dto">修改登录密码</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "self")]
        public async Task<Result> ModifyPassword([FromBody] AccountModifyPasswordRequestDto dto)
        {
            int id = HttpContext.User.UserId();
            var service = this._serviceProvider.GetRequiredService<AccountPasswordChangeService>();
            var result = await service.ModifyAsync(id, dto.Password, dto.OldPassword);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.AccountModifyPassword, $"{id}\r\n" + dto.ToString(), "Success");  //记录日志
            await _accountRepository.ModifyAsync(result.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改账户信息
        /// </summary>
        /// <param name="dto">账户信息</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "self")]
        public async Task<Result> Midify([FromBody]AccountModifyInfoRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //调用服务修改账户数据
            int id = HttpContext.User.UserId();
            var service = this._serviceProvider.GetRequiredService<AccountInfoChangeService>();
            var modifyResult = await service.Modify(id, dto.Name, dto.Avatar, dto.Status);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.AccountModifyStatus, $"{id}\r\n" + dto.ToString(), "Success");  //记录日志
            await _accountRepository.ModifyAsync(modifyResult.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改邮箱地址
        /// </summary>
        /// <param name="dto">邮箱地址</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "self")]
        public async Task<Result> ModifyEmail([FromBody] AccountModifyEmailRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            int id = HttpContext.User.UserId();
            var service = this._serviceProvider.GetRequiredService<AccountEmailChangeService>();
            var modifyResult = service.Modify(id, dto.Email);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.AccountModifyEmail, $"{id}\r\n" + dto.ToString(), "Success");  //记录日志
            await _accountRepository.ModifyAsync(modifyResult.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改电话号码
        /// </summary>
        /// <param name="dto">电话号码</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "self")]
        public async Task<Result> ModifyTelephone([FromBody] AccountModifyTelephoneRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //调用服务修改电话号码
            int id = HttpContext.User.UserId();
            var service = this._serviceProvider.GetRequiredService<AccountTelephoneChangeService>();
            var modifyResult = service.Modify(id, new PhoneNumber(dto.Number, dto.AreaCode));
            if (!modifyResult.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.AccountModifyTelephone, $"{id}\r\n" + dto.ToString(), "Success");  //记录日志
            await _accountRepository.ModifyAsync(modifyResult.Data);
            
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result.ReSuccess();
        }


    }
}
