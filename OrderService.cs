using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersFilterApp
{
    public class OrderService
    {
        private readonly Logger _logger;

        public OrderService(Logger logger)
        {
            _logger = logger;
        }

        public List<Order> LoadOrders(string filePath)
        {
            var orders = new List<Order>();
            var lines = File.ReadAllLines(filePath);
            int lineNumber = 0;

            foreach (var line in lines)
            {
                lineNumber++;
                try
                {
                    var order = Order.FromString(line);
                    if (orders.Any(o => o.OrderNumber == order.OrderNumber))
                    {
                        _logger.LogError($"Строка {lineNumber}: Заказ с номером {order.OrderNumber} уже существует");
                        continue;
                    }
                    orders.Add(order);
                    _logger.Log($"Заказ {order.OrderNumber} считан успешно.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Строка {lineNumber}: {ex.Message}.");
                }
            }

            _logger.Log($"Загружено {orders.Count} корректных заказов из файла {filePath}");
            return orders;
        }

        public List<Order> FilterOrdersByDistrictAndTime(List<Order> orders, string district, DateTime firstOrderTime)
        {
            DateTime timeLimit = firstOrderTime.AddMinutes(30);
            var filteredOrders = orders
                .Where(order => order.District == district && order.DeliveryTime >= firstOrderTime && order.DeliveryTime <= timeLimit)
                .ToList();

            _logger.Log($"Найдено {filteredOrders.Count} заказов для района {district} " +
                $"в период с {firstOrderTime.ToString("yyyy-MM-dd HH:mm:ss")} по {timeLimit.ToString("yyyy-MM-dd HH:mm:ss")}");
            return filteredOrders;
        }

        public void SaveFilteredOrders(List<Order> orders, string filePath)
        {
            var lines = new List<string>();
            lines.AddRange(orders.Select(o => $"{o.OrderNumber},{o.Weight},{o.District},{o.DeliveryTime.ToString("yyyy-MM-dd HH:mm:ss")}"));
            File.WriteAllLines(filePath, lines);
            _logger.Log($"Сохранено {orders.Count} заказов в файл {filePath}");
        }
    }

}
