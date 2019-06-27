using System;

namespace TaskGroupWeb.Helpers
{
    public class TaskCodeGenerator
    {
        public static string Generate(int taskId)
        {
            return string.Format("{0}-{1}-{2}", DateTime.Now.Year, DateTime.Now.Month.ToString("00"), taskId.ToString("00000"));
        }
    }
}
