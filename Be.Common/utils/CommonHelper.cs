using Newtonsoft.Json;

namespace Be.Common.utils
{
    public static class CommonHelper
    {
        public static T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }    

        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string GetUniqueFileName(List<string> list, string fileName)
        {
            var fileNameNoExtension = Path.GetFileNameWithoutExtension(fileName);
            var extension = Path.GetExtension(fileName);
            var start = 1;
            var lowers = list.Select(x => x.ToLower()).ToArray();
            while(lowers.Contains(fileName.ToLower()))
            {
                fileName = $"{fileNameNoExtension}({start}){extension}";
                start++;
            }
            return fileName;
        }
    }
}
