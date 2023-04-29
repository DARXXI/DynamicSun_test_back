using Weather.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Weather.Repository.Configurations
{
    public class WeatherConfig : IEntityTypeConfiguration<Weather.Domain.Entities.Weather>
    {
        public void Configure(EntityTypeBuilder<Weather.Domain.Entities.Weather> builder)
        {
            builder
                .Property(t => t.Pressure)
                .HasColumnName("Давление")
                .HasColumnType("int");
            builder
                .Property(t => t.Note)  
                .HasMaxLength(100)
                .HasColumnName("Погодные явления")
                .HasColumnType("varchar");
            builder
                .Property(t => t.Date)
                .HasColumnName("Число");
            builder
                .Property(t => t.Cloudness)
                .HasColumnName("Облачность");
            builder
                .Property(t => t.DewPoint)
                .HasColumnName("Точка росы");
            builder
                .Property(t => t.Time)
                .HasColumnName("Время");
            builder
                .Property(t => t.WindDirection)
                .HasColumnName("Направление ветра");
            builder
                .Property(t => t.WindSpeed)
                .HasColumnName("Скорость ветра");
            builder
                .Property(t => t.LowCloudBoundary)
                .HasColumnName("Нижняя граница облачности");
            builder
                .Property(t => t.Humidity)
                .HasColumnName("Влажность");
            builder
                .Property(t => t.Visibility)
                .HasColumnName("Видимость");
            builder
                .Property(t => t.Temprature)
                .HasColumnName("Температура");
        }
    }
}