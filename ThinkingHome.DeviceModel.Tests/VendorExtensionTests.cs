using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Tests;

public class VendorExtensionTests
{
    [Fact]
    public void Vendor_marks_cover_full_concept_sets()
    {
        // Типы одного концепта связаны каноническим именем (правило деривации идентификаторов):
        // WaterMeterProperty и WaterMeterState → "WaterMeter". Пометка [VendorExtension] обязана
        // покрывать набор целиком — сверка словаря с Matter пропускает помеченное, и непомеченная
        // половина набора сломала бы её. Члены DeviceType здесь не участвуют: типы устройств и
        // кластеры — разные пространства Matter, их пометки проверит сверка по device_types XML.
        var hierarchies = new[] { typeof(Capability), typeof(Property), typeof(DeviceCommand), typeof(StateValue) };

        var concepts = hierarchies
            .SelectMany(baseType => baseType.Assembly.GetTypes()
                .Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t)))
            .GroupBy(ConceptName);

        foreach (var concept in concepts)
        {
            var marked = concept
                .Where(t => t.GetCustomAttributes(typeof(VendorExtensionAttribute), false).Length > 0)
                .ToArray();

            Assert.True(marked.Length == 0 || marked.Length == concept.Count(),
                $"Концепт «{concept.Key}»: [VendorExtension] стоит на [{string.Join(", ", marked.Select(t => t.Name))}], " +
                $"но не на [{string.Join(", ", concept.Except(marked).Select(t => t.Name))}]");
        }
    }

    // каноническое имя концепта — имя типа без суффикса иерархии (как в правиле деривации идентификаторов)
    private static string ConceptName(Type type)
    {
        var name = type.Name;
        foreach (var suffix in new[] { "Capability", "Property", "Command", "State" })
        {
            if (name.EndsWith(suffix))
            {
                name = name[..^suffix.Length];
                break;
            }
        }

        return name;
    }
}
