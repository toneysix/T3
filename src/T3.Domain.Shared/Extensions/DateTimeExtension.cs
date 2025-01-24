namespace T3.Domain.Shared.Extensions;

public static class DateTimeExtension
{
    /// <summary>
    /// Определяет находится ли дата и время в будущем.
    /// </summary>
    /// <param name="dateTime">Дата/время</param>
    /// <returns>Возвращает true в случае нахождения даты и времени в будущем, в противном случе false</returns>
    public static bool IsDateTimeInFuture(this DateTime dateTime)
    {
        return dateTime > DateTime.Now;
    }
}