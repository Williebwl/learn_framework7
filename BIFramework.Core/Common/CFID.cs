﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    public class CFID
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Guid NewGuid()
        {
            byte[] uid = Guid.NewGuid().ToByteArray();
            byte[] binDate = BitConverter.GetBytes(DateTime.UtcNow.Ticks);

            byte[] secuentialGuid = new byte[uid.Length];

            secuentialGuid[0] = uid[0];
            secuentialGuid[1] = uid[1];
            secuentialGuid[2] = uid[2];
            secuentialGuid[3] = uid[3];
            secuentialGuid[4] = uid[4];
            secuentialGuid[5] = uid[5];
            secuentialGuid[6] = uid[6];
            // set the first part of the 8th byte to '1100' so     
            // later we'll be able to validate it was generated by us   

            secuentialGuid[7] = (byte)(0xc0 | (0xf & uid[7]));

            // the last 8 bytes are sequential,    
            // it minimizes index fragmentation   
            // to a degree as long as there are not a large    
            // number of Secuential-Guids generated per millisecond  

            secuentialGuid[9] = binDate[0];
            secuentialGuid[8] = binDate[1];
            secuentialGuid[15] = binDate[2];
            secuentialGuid[14] = binDate[3];
            secuentialGuid[13] = binDate[4];
            secuentialGuid[12] = binDate[5];
            secuentialGuid[11] = binDate[6];
            secuentialGuid[10] = binDate[7];

            return new Guid(secuentialGuid);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static long NewID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            long longGuid = BitConverter.ToInt64(buffer, 0);

            string value = Math.Abs(longGuid).ToString();

            var buf = new byte[value.Length];
            int p = 0;
            for (int i = 0; i < value.Length; )
            {
                byte ph = Convert.ToByte(value[i]);

                int fix = 1;
                if ((i + 1) < value.Length)
                {
                    byte pl = Convert.ToByte(value[i + 1]);
                    buf[p] = (byte)((ph << 4) + pl);
                    fix = 2;
                }
                else
                {
                    buf[p] = (ph);
                }

                if ((i + 3) < value.Length)
                {
                    if (Convert.ToInt16(value.Substring(i, 3)) < 256)
                    {
                        buf[p] = Convert.ToByte(value.Substring(i, 3));
                        fix = 3;
                    }
                }
                p++;
                i = i + fix;
            }
            var buf2 = new byte[p];
            for (int i = 0; i < p; i++)
            {
                buf2[i] = buf[i];
            }
            //JSON Number最大支持9007199254740992
            //string guid2Int = BitConverter.ToInt32(buf2, 0).ToString().Replace("-", "").Replace("+", "");
            //guid2Int = guid2Int.Length >= 9 ? guid2Int.Substring(0, 9) : guid2Int.PadLeft(9, '0');
            //return Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmm") + guid2Int);

            string year2Int = ((int)(DateTime.Now - DateTime.Parse("2015-12-31 00:00")).TotalMinutes).ToString();
            year2Int = year2Int.Length >= 7 ? year2Int.Substring(0, 7) : year2Int.PadLeft(7, '0');
            string guid2Int = BitConverter.ToInt32(buf2, 0).ToString().Replace("-", "").Replace("+", "");
            guid2Int = guid2Int.Length >= 9 ? guid2Int.Substring(0, 9) : guid2Int.PadLeft(9, '0');
            return Convert.ToInt64(year2Int + guid2Int);
        }
    }
}
