using System.Dynamic;
using System.Reflection;

namespace NTemplate
{
    public class DynamicObjectWrapper : DynamicObject
    {
        private readonly object instance;

        public DynamicObjectWrapper(object instance) => this.instance = instance ?? this;

        public override bool TryGetMember(GetMemberBinder binder, out object result) => GetProperty(instance, binder.Name, out result);

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = System.Convert.ChangeType(instance, binder.Type);
            return true;
        }

        private bool GetProperty(object instance, string name, out object result)
        {
            var memberInfos = instance.GetType().GetMember(name, BindingFlags.Public
                                                             | BindingFlags.NonPublic
                                                             | BindingFlags.GetProperty
                                                             | BindingFlags.Instance);
            if (memberInfos != null && memberInfos.Length > 0)
            {
                var mi = memberInfos[0];
                if (mi.MemberType == MemberTypes.Property)
                {
                    result = ((PropertyInfo)mi).GetValue(instance, null);
                    return true;
                }
            }

            result = null;
            return false;
        }
    }
}
