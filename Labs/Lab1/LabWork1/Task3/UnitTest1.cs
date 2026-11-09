using System.Data;
using Task2.Services;
namespace Task3
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            DataView dataView = null;
            DataService.ExportData(dataView);
        }
    }
}
