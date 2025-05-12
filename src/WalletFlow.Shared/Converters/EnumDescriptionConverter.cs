using System.ComponentModel;
using System.Reflection;

namespace WalletFlow.Shared.Converters;

public static class EnumDescriptionConverter
{
    public static string ToDescription(this Enum value)
    {
        var fi = value.GetType().GetField(value.ToString())!;
        var attr = fi.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? value.ToString();
    }

    public static T FromDescription<T>(string description) where T : Enum
    {
        var type = typeof(T);
        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attr = field.GetCustomAttribute<DescriptionAttribute>();
            if ((attr != null && attr.Description == description)
                || field.Name == description)
                return (T)field.GetValue(null)!;
        }
        throw new ArgumentException($"'{description}' não corresponde a nenhuma descrição de {type.Name}");
    }
}