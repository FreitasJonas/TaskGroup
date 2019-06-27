using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using static Objetos.DbEnumerators;

namespace TaskGroupWeb.Helpers
{
    public class HtmlHelpers
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

            if (topValue <= enumValues.Length)
            {
                var topText = enumValues.GetValue(topValue - 1);

                selectList.Add(new SelectListItem()
                {
                    Text = topText.ToString(),
                    Value = topValue.ToString()
                });
            }

            foreach (int value in enumValues)
            {
                if (value == topValue) { continue; }

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

        public static string GetBreadCrumb(params BreadCrumbItem[] items)
        {
            var html = "<ul class=\"breadcrumb\">";

            for (int i = 0; i < items.Length; i++)
            {
                if (i == items.Length - 1)
                {
                    //ultimo
                    html += "<li><a href=\"" + items[i].link + "\">" + items[i].title + "</a> </li>";
                }
                else
                {
                    html += "<li><a href=\"" + items[i].link + "\" > " + items[i].title + "</a> &nbsp;&nbsp; \\  &nbsp;&nbsp; </li>";
                }                
            }
            
            html += "</ul>";
            return html;
        }

        public static string GetColorToStatus(TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.Aberto:
                    return "#787878";
                case TaskStatus.Andamento:
                    return "#8f8f00";
                case TaskStatus.Finalizado:
                    return "#2b8f00";
                default: return status.ToString();
            }
        }

        public static string GetIconToStatus(TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.Aberto:
                    return "envelope";
                case TaskStatus.Andamento:
                    return "running";
                case TaskStatus.Finalizado:
                    return "check";
                default: return status.ToString();
            }
        }
    }

    public class BreadCrumbItem
    {
        public string title;
        public string link;

        public BreadCrumbItem(string _title, string _link)
        {
            this.title = _title;
            this.link = _link;
        }
    }
}
