namespace System.ComponentModel.DataAnnotations
{
    public static class ValidatorExtensions
    {
        public static Result Valid(this object obj)
        {
            return ValidationHelper.ValidResult(obj);
        }

        public static Result<T> Valid<T>(this object obj)
        {
            var result = ValidationHelper.ValidResult(obj);
            return Result<T>.ReSuccess((T)obj);
        }
    }
}
