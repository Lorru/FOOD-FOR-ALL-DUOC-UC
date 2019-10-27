using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class SystemService
    {
        public SystemService()
        {

        }

        public bool comparator(string condition, object a, object b)
        {
            bool validate = false;

            if (a != null)
            {
                if (a.GetType() == typeof(string))
                {
                    switch (condition)
                    {
                        case "==": validate = a.ToString().ToLower() == b.ToString().ToLower(); break;
                        case "like": validate = a.ToString().ToLower().Contains(b.ToString().ToLower()); break;
                        default: validate = a.ToString().Contains(b.ToString()); break;
                    }
                }
                else if (a.GetType() == typeof(int))
                {
                    switch (condition)
                    {
                        case "==": validate = Convert.ToInt32(a) == Convert.ToInt32(b); break;
                        case "<": validate = Convert.ToInt32(a) < Convert.ToInt32(b); break;
                        case "<=": validate = Convert.ToInt32(a) <= Convert.ToInt32(b); break;
                        case ">": validate = Convert.ToInt32(a) > Convert.ToInt32(b); break;
                        case ">=": validate = Convert.ToInt32(a) >= Convert.ToInt32(b); break;
                        default: validate = Convert.ToInt32(a) == Convert.ToInt32(b); break;
                    }
                }
                else if (a.GetType() == typeof(double))
                {
                    switch (condition)
                    {
                        case "==": validate = Convert.ToDouble(a) == Convert.ToDouble(b); break;
                        case "<": validate = Convert.ToDouble(a) < Convert.ToDouble(b); break;
                        case "<=": validate = Convert.ToDouble(a) <= Convert.ToDouble(b); break;
                        case ">": validate = Convert.ToDouble(a) > Convert.ToDouble(b); break;
                        case ">=": validate = Convert.ToDouble(a) >= Convert.ToDouble(b); break;
                        default: validate = Convert.ToDouble(a) == Convert.ToDouble(b); break;
                    }
                }
                else if (a.GetType() == typeof(DateTime))
                {
                    switch (condition)
                    {
                        case "==": validate = Convert.ToDateTime(a) == Convert.ToDateTime(b); break;
                        case "<": validate = Convert.ToDateTime(a) < Convert.ToDateTime(b); break;
                        case "<=": validate = Convert.ToDateTime(a) <= Convert.ToDateTime(b); break;
                        case ">": validate = Convert.ToDateTime(a) > Convert.ToDateTime(b); break;
                        case ">=": validate = Convert.ToDateTime(a) >= Convert.ToDateTime(b); break;
                        default: validate = Convert.ToDateTime(a) == Convert.ToDateTime(b); break;
                    }
                }
                else if (a.GetType() == typeof(bool))
                {
                    switch (condition)
                    {
                        case "==": validate = Convert.ToBoolean(a) == Convert.ToBoolean(b); break;
                        default: validate = Convert.ToBoolean(a) == Convert.ToBoolean(b); break;
                    }
                }

            }

            return validate;
        }
    }
}
