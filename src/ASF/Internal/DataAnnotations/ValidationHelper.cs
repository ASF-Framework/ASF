using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace System.ComponentModel.DataAnnotations
{
    public class EntityValidationResult
    {
        public IList<ValidationResult> Errors { get; private set; }
        public bool HasError
        {
            get { return Errors.Count > 0; }
        }

        public EntityValidationResult(IList<ValidationResult> errors = null)
        {
            Errors = errors ?? new List<ValidationResult>();
        }
    }
    public class EntityValidator<T> where T : class
    {
        public EntityValidationResult Validate(T entity)
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject
                    (entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }
    public class ValidationHelper
    {
        public static EntityValidationResult ValidateEntity<T>(T entity)
            where T : class
        {
            return new EntityValidator<T>().Validate(entity);
        }

        /// <summary>
        /// 验证是否成功
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool IsValid<T>(T entity)
            where T : class
        {
            return IsValid(entity, null);
        }
        /// <summary>
        /// 验证是否成功
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="logger">日志记录</param>
        /// <param name="logLevel">日志记录级别</param>
        /// <returns></returns>
        public static bool IsValid<T>(T entity, ILogger logger, LogLevel logLevel = LogLevel.Information)
            where T : class
        {
            EntityValidationResult result = ValidateEntity<T>(entity);

            if (result.HasError && logger != null)
                logger.Log(logLevel, 0, new FormattedLogValues("{EntityType} Valid Fail Message:{error}", entity.GetType(), JsonConvert.SerializeObject(result.Errors)), null, (object state, Exception error) =>
              {
                  return state.ToString();
              });
            return !result.HasError;
        }

        /// <summary>
        /// 验证是否成功
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static Result ValidResult<T>(T entity)
            where T : class
        {
            EntityValidationResult result = ValidateEntity<T>(entity);
            if (result.HasError)
                return Result.ReFailure(BaseResultCodes.BadRequest.ToFormat(result.Errors[0].ErrorMessage));
            else
                return Result.ReSuccess();
        }


    }
}
