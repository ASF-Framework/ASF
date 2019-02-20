using ASF.Application.DTO;
using ASF.Domain;
using ASF.Domain.Services;
using ASF.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ASF.Application
{
    /// <summary>
    /// 日志服务
    /// </summary>
    [Authorize]
    public class LoggerController : Controller
    {
        private readonly ILoggingRepository _loggingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly LogOperateRecordService _operateLog;

        public LoggerController(ILoggingRepository loggingRepository, IUnitOfWork unitOfWork, LogOperateRecordService operateLog)
        {
            _loggingRepository = loggingRepository;
            _unitOfWork = unitOfWork;
            _operateLog = operateLog;
        }

        /// <summary>
        /// 分页获取所有的日志集合
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultPagedList<AccountInfoBaseResponseDto>> GetList([FromBody]LoggerListPagedRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return ResultPagedList<AccountInfoBaseResponseDto>.ReFailure(result);

            var logList = await _loggingRepository.GetList(dto);
            var logings = Mapper.Map<IList<AccountInfoBaseResponseDto>>(logList.Loggings);
            return ResultPagedList<AccountInfoBaseResponseDto>.ReSuccess(logings, logList.TotalCount);
        }

        /// <summary>
        /// 根据指定时间删除日志（三个月之内的日志不能删除）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Delete([FromBody] LoggerDeleteRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;


            //三个月之内的日志不能删除
            if (dto.BeginTime > DateTime.Now.AddMonths(-3))
                return Result.ReFailure(ResultCodes.LogginDeletedCannoBeWithinThreeMonths);

            //数据持久化
            _operateLog.Record(ASFPermissions.LoggingDelete, dto.ToString(), "Success");  //记录日志
            await _loggingRepository.Delete(dto);
            await _unitOfWork.CommitAsync();
            return Result.ReSuccess();
        }
    }
}
