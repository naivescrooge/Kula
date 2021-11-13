﻿using Kula.Data.Container;
using Kula.Util;
using System.Collections.Generic;

namespace Kula.Data.Type
{
    public class DuckType : IType
    {
        private readonly Dictionary<string, IType> pairs = new Dictionary<string, IType>();
        private readonly int hash;
        private readonly string name;

        public DuckType(string name, KulaEngine root, IList<(string, string)> nodelist)
        {
            hash = 0;
            int len = nodelist.Count;
            var duck_type_dict = root.DuckTypeDict;
            for (int i = 0; i < len; ++i)
            {
                var kk = nodelist[i].Item1;
                var vv = nodelist[i].Item2;
                if (RawType.TypeDict.ContainsKey(vv))
                {
                    pairs[kk] = RawType.TypeDict[vv];
                }
                else
                {
                    if (duck_type_dict.ContainsKey(vv))
                        pairs[kk] = duck_type_dict[vv];
                    else
                        // 没这个鸭子类型
                        throw new KulaException.KTypeException(vv);
                }
                hash = hash * 17 + kk.GetHashCode() * 7 + vv.GetHashCode();
            }
            this.name = name;
        }

        public override int GetHashCode() { return hash; }

        private bool CheckDuck(Map map)
        {
            var map_data = map.Data;
            foreach (var dd in pairs)
            {
                if (!map_data.ContainsKey(dd.Key))
                    return false;
                if (!dd.Value.Check(map_data[dd.Key]))
                    return false;
            }
            return true;
        }

        System.Type IType.ToType { get => throw new System.NotImplementedException(); }

        DuckType IType.ToDuck { get => this; }

        bool IType.IsDuck { get => true; }

        public bool Check(object o) => o is Map o_map && CheckDuck(o_map);

        public override string ToString() => name;
    }
}
