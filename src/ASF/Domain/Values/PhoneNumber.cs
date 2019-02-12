using System;
using System.ComponentModel.DataAnnotations;

namespace ASF.Domain.Values
{
    /// <summary>
    /// 手机号码
    /// </summary>
    public class PhoneNumber : IEquatable<PhoneNumber>,IValueObject
    {
        public PhoneNumber(string number, int areacode = 86)
        {
            this.Number = number;
            this.AreaCode = areacode;
        }

        public PhoneNumber(string phoneNumber)
        {
            var data = phoneNumber.Split('+');
            if (data.Length != 2)
            {
                this.Number = "";
            }
            if (data.Length > 0)
                this.Number = data[1];
            if (data.Length > 1)
                this.AreaCode = int.Parse(data[0]);
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        [MaxLength(20)]
        public string Number { get; }
        /// <summary>
        /// 手机号码区号
        /// </summary>
        public int AreaCode { get; }

        public override string ToString()
        {
            return this.AreaCode + "+" + this.Number;
        }
        public PhoneNumber Clone()
        {
            return new PhoneNumber(this.Number, this.AreaCode);
        }
        public bool Equals(PhoneNumber other)
        {
            if (this.AreaCode != other.AreaCode)
                return false;
            if (this.Number != other.Number)
                return false;
            else
                return true;
        }
    }
}
