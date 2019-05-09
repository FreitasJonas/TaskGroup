using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskGroupWeb.Helpers
{
    public class HtmlDropDownHelper
    {
        public static SelectList GetDropDownList<T>(List<T> sourceList, string propValue, string propText, string firstValue = "")
        {
            var selectList = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(firstValue) && firstValue != "0")
            {
                var obj = sourceList.FirstOrDefault(o => o.GetType().GetProperty(propValue).GetValue(o).ToString() == firstValue.ToUpper());

                if (obj != null)
                {
                    selectList.Add(new SelectListItem()
                    {
                        Text = obj.GetType().GetProperty(propText).GetValue(obj).ToString(),
                        Value = obj.GetType().GetProperty(propValue).GetValue(obj).ToString()
                    });

                    sourceList.Remove(obj);
                }
                else
                {
                    selectList.Add(new SelectListItem()
                    {
                        Text = "-- Selecione --",
                        Value = ""
                    });
                }
            }
            else
            {
                selectList.Add(new SelectListItem()
                {
                    Text = "-- Selecione --",
                    Value = ""
                });
            }

            foreach (var item in sourceList)
            {
                selectList.Add(new SelectListItem()
                {
                    Text = item.GetType().GetProperty(propText).GetValue(item).ToString(),
                    Value = item.GetType().GetProperty(propValue).GetValue(item).ToString()
                });
            }

            var retorno = new SelectList(
              selectList,
              "Value",
              "Text"
            );

            return retorno;
        }

        public static SelectList GetDropDownFromEnum<TEnum>(int topValue) where TEnum : struct
        {
            var enumerationType = typeof(TEnum);

            if (!enumerationType.IsEnum)
                throw new ArgumentException("Enumerador esperado.");

            var selectList = new List<SelectListItem>();

            var enumValues = Enum.GetValues(enumerationType);

            //if (topValue < enumValues.Length)
            //{
            //    var topText = enumValues.GetValue(topValue);

            //    selectList.Add(new SelectListItem()
            //    {
            //        Text = topText.ToString(),
            //        Value = topValue.ToString()
            //    });
            //}

            foreach (int value in enumValues)
            {
                //if (value == topValue) { continue; }

                var name = Enum.GetName(enumerationType, value);

                selectList.Add(new SelectListItem()
                {
                    Text = name,
                    Value = value.ToString()
                });
            }

            var retorno = new SelectList(
              selectList,
              "Value",
              "Text"
            );

            return retorno;
        }
    }
}
