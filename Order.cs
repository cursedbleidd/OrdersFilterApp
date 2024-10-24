using System.Globalization;

namespace OrdersFilterApp
{
    public class Order
    {
        public int OrderNumber { get; set; }
        public double Weight { get; set; }
        public string District { get; set; }
        public DateTime DeliveryTime { get; set; }

        public Order(int orderNumber, double weight, string district, DateTime deliveryTime)
        {
            if (weight <= .0)
                throw new ArgumentException($"Некорректное значение веса: {weight}");

            OrderNumber = orderNumber;
            Weight = weight;
            District = district;
            DeliveryTime = deliveryTime;
        }

        public static Order FromString(string line)
        {
            string[] values = line.Split(',');
            if (values.Length != 4)
                throw new FormatException("Неверное количество полей в строке");

            if (!int.TryParse(values[0], out int number))
                throw new FormatException($"Некорректный формат номера заказа: {values[0]}");

            if (!double.TryParse(values[1], out double weight))
                throw new FormatException($"Некорректный формат веса: {values[1]}");

            if (!TryParseDate(values[3], out DateTime deliveryTime))
                throw new FormatException($"Некорректный формат времени доставки: {values[3]}");

            if (values[2].Length < 3)
                throw new FormatException($"Некорректный формат района: {values[2]}");

            return new Order(number, weight, values[2], deliveryTime);
        }
        public static bool TryParseDate(string date, out DateTime datetime)
        {
            return DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out datetime);
        }
    }
}
