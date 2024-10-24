using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Linq;
namespace OrdersFilterApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 5)
            {
                Console.WriteLine("Неверные входные параметры: <файл заказов> <район> <время первой доставки> <файл логов> <файл результата>");
                return;
            }

            string ordersFilePath = args[0];
            string district = args[1];    
            string logFilePath = args[3];
            string resultFilePath = args[4];

            DateTime firstDeliveryTime;

            if (!File.Exists(ordersFilePath))
            {
                Console.WriteLine($"Файл заказов не существует: {ordersFilePath}");
                return;
            }
            if (!IsFilePathValid(resultFilePath))
            {
                Console.WriteLine($"Неверный формат пути к выходному файлу: {resultFilePath}");
                return;
            }
            if (!IsFilePathValid(logFilePath))
            {
                Console.WriteLine($"Неверный формат пути к файлу логов: {logFilePath}");
                return;
            }
            if (!Order.TryParseDate(args[2], out firstDeliveryTime))
            {
                Console.WriteLine($"Неправильный формат даты-времени: {args[2]}");
                return;
            }

            Logger logger = new Logger(logFilePath);
            OrderService orderService = new OrderService(logger);
            try
            {
                var orders = orderService.LoadOrders(ordersFilePath);

                var filteredOrders = orderService.FilterOrdersByDistrictAndTime(orders, district, firstDeliveryTime);

                orderService.SaveFilteredOrders(filteredOrders, resultFilePath);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        static bool IsFilePathValid(string filepath) => 
            !Path.GetInvalidFileNameChars().Any(ch => Path.GetFileName(filepath).Contains(ch)) && filepath.Length > 0 && 
            (Directory.Exists(Path.GetDirectoryName(filepath)) || Path.GetDirectoryName(filepath) == string.Empty);
    }

}
