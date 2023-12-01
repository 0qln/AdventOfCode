using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTreeGenerator
{
    public static class Extensions
    {
        public static string? GetParent(this string dir) => Directory.GetParent(dir)?.ToString();
        public static string? GetParent(this DirectoryInfo dir) => Directory.GetParent(dir.ToString())?.ToString();
    }
}
