﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using ChaYeFeng;
using TestMEF;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
//            string dataStr = @"
//
//软件版本	2.04.11
//
//
//
//实验文件路径:	
//方案文件路径:	
//
//
//
//板编号	板 1
//日期	2017/7/5 星期三
//时间	15:04:48
//检测仪类型:	ELx800
//检测仪序列号:	Unknown
//检测类型	模拟
//
//程序详细信息
//
//板类型	96 WELL PLATE
//检测	吸收光 终点
//	全板
//	波长:  405, 630
//	检测速度: 正常
//
//布局
//	1	2	3	4	5	6	7	8	9	10	11	12
//A	BLK	SPL5	SPL13	SPL21	SPL29	SPL37	SPL45	SPL53	SPL61	SPL69	SPL77	SPL85	孔 ID
//B	NC	SPL6	SPL14	SPL22	SPL30	SPL38	SPL46	SPL54	SPL62	SPL70	SPL78	SPL86	孔 ID
//C	QC	SPL7	SPL15	SPL23	SPL31	SPL39	SPL47	SPL55	SPL63	SPL71	SPL79	SPL87	孔 ID
//D	PC	SPL8	SPL16	SPL24	SPL32	SPL40	SPL48	SPL56	SPL64	SPL72	SPL80	SPL88	孔 ID
//E	SPL1	SPL9	SPL17	SPL25	SPL33	SPL41	SPL49	SPL57	SPL65	SPL73	SPL81	SPL89	孔 ID
//F	SPL2	SPL10	SPL18	SPL26	SPL34	SPL42	SPL50	SPL58	SPL66	SPL74	SPL82	SPL90	孔 ID
//G	SPL3	SPL11	SPL19	SPL27	SPL35	SPL43	SPL51	SPL59	SPL67	SPL75	SPL83	SPL91	孔 ID
//H	SPL4	SPL12	SPL20	SPL28	SPL36	SPL44	SPL52	SPL60	SPL68	SPL76	SPL84	SPL92	孔 ID
//
//结果
//	1	2	3	4	5	6	7	8	9	10	11	12
//A	2.120	2.150	2.180	2.210	2.240	2.270	2.300	2.330	2.350	2.380	2.410	2.440	405
//	2.220	2.250	2.280	2.310	2.340	2.370	2.400	2.430	2.450	2.480	2.510	2.540	630
//	0.000	0.030	0.060	0.090	0.120	0.150	0.180	0.210	0.230	0.260	0.290	0.320	本底 405
//	0.000	0.030	0.060	0.090	0.120	0.150	0.180	0.210	0.230	0.260	0.290	0.320	本底 630
//	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	增量
//	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	CUTOFF
//	-	-	-	-	-	-	-	-	-	-	-	-	符号 (CUTOFF)
//B	2.140	2.170	2.200	2.230	2.260	2.290	2.310	2.340	2.370	2.400	2.430	2.450	405
//	2.240	2.270	2.300	2.330	2.360	2.390	2.410	2.440	2.470	2.500	2.530	2.550	630
//	0.020	0.050	0.080	0.110	0.140	0.170	0.190	0.220	0.250	0.280	0.310	0.330	本底 405
//	0.020	0.050	0.080	0.110	0.140	0.170	0.190	0.220	0.250	0.280	0.310	0.330	本底 630
//	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	增量
//	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	CUTOFF
//	-	-	-	-	-	-	-	-	-	-	-	-	符号 (CUTOFF)
//C	2.150	2.180	2.210	2.240	2.270	2.300	2.330	2.360	2.390	2.420	2.440	2.470	405
//	2.250	2.280	2.310	2.340	2.370	2.400	2.430	2.460	2.490	2.520	2.540	2.570	630
//	0.030	0.060	0.090	0.120	0.150	0.180	0.210	0.240	0.270	0.300	0.320	0.350	本底 405
//	0.030	0.060	0.090	0.120	0.150	0.180	0.210	0.240	0.270	0.300	0.320	0.350	本底 630
//	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	增量
//	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	CUTOFF
//	-	-	-	-	-	-	-	-	-	-	-	-	符号 (CUTOFF)
//D	2.170	2.200	2.230	2.260	2.290	2.320	2.350	2.380	2.400	2.430	2.460	2.480	405
//	2.270	2.300	2.330	2.360	2.390	2.420	2.450	2.480	2.500	2.530	2.560	2.580	630
//	0.050	0.080	0.110	0.140	0.170	0.200	0.230	0.260	0.280	0.310	0.340	0.360	本底 405
//	0.050	0.080	0.110	0.140	0.170	0.200	0.230	0.260	0.280	0.310	0.340	0.360	本底 630
//	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	增量
//	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	CUTOFF
//	-	-	-	-	-	-	-	-	-	-	-	-	符号 (CUTOFF)
//E	2.190	2.220	2.250	2.280	2.310	2.340	2.370	2.390	2.420	2.450	2.470	2.500	405
//	2.290	2.320	2.350	2.380	2.410	2.440	2.470	2.490	2.520	2.550	2.570	2.600	630
//	0.070	0.100	0.130	0.160	0.190	0.220	0.250	0.270	0.300	0.330	0.350	0.380	本底 405
//	0.070	0.100	0.130	0.160	0.190	0.220	0.250	0.270	0.300	0.330	0.350	0.380	本底 630
//	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	增量
//	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	CUTOFF
//	-	-	-	-	-	-	-	-	-	-	-	-	符号 (CUTOFF)
//F	2.210	2.240	2.270	2.300	2.330	2.350	2.380	2.410	2.440	2.460	2.490	2.510	405
//	2.310	2.340	2.370	2.400	2.430	2.450	2.480	2.510	2.540	2.560	2.590	2.610	630
//	0.090	0.120	0.150	0.180	0.210	0.230	0.260	0.290	0.320	0.340	0.370	0.390	本底 405
//	0.090	0.120	0.150	0.180	0.210	0.230	0.260	0.290	0.320	0.340	0.370	0.390	本底 630
//	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	增量
//	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	CUTOFF
//	-	-	-	-	-	-	-	-	-	-	-	-	符号 (CUTOFF)
//G	2.230	2.260	2.290	2.310	2.340	2.370	2.400	2.430	2.450	2.480	2.500	2.530	405
//	2.330	2.360	2.390	2.410	2.440	2.470	2.500	2.530	2.550	2.580	2.600	2.630	630
//	0.110	0.140	0.170	0.190	0.220	0.250	0.280	0.310	0.330	0.360	0.380	0.410	本底 405
//	0.110	0.140	0.170	0.190	0.220	0.250	0.280	0.310	0.330	0.360	0.380	0.410	本底 630
//	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	增量
//	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	CUTOFF
//	-	-	-	-	-	-	-	-	-	-	-	-	符号 (CUTOFF)
//H	2.240	2.270	2.300	2.330	2.360	2.390	2.420	2.440	2.470	2.490	2.520	2.540	405
//	2.340	2.370	2.400	2.430	2.460	2.490	2.520	2.540	2.570	2.590	2.620	2.640	630
//	0.120	0.150	0.180	0.210	0.240	0.270	0.300	0.320	0.350	0.370	0.400	0.420	本底 405
//	0.120	0.150	0.180	0.210	0.240	0.270	0.300	0.320	0.350	0.370	0.400	0.420	本底 630
//	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	0.000	增量
//	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	0.800	CUTOFF
//	-	-	-	-	-	-	-	-	-	-	-	-	符号 (CUTOFF)
//
//Cutoffs (CUTOFF)
//
//公式	值
//1	1.000
//";
//            string lineSplitChar = @"CUTOFF
//	-	-	-	-	-	-	-	-	-	-	-	-	符号 (CUTOFF)";
//            int layoutStart = dataStr.IndexOf("布局");
//            int resultStart = dataStr.IndexOf("结果");
//            string layoutStr = dataStr.Substring(layoutStart, resultStart - layoutStart).Substring(33);
//            string[] layoutArray = layoutStr.Replace("\r\n","").Split(new string[] { "孔 ID" },StringSplitOptions.None);
//            string[] resultArray = dataStr.Substring(resultStart + 33).Split(new string[] { lineSplitChar }, StringSplitOptions.RemoveEmptyEntries);
//            int tempStartIndex = 0;
//            Regex reg = new Regex(@"[A-Za-z]");
//            Match match = Match.Empty;
//            string tempStr = string.Empty;
//            string[] tempArray = null;
//            string[] itemNameRow = null;
//            string[] itemResultRow = null;
//            for (int i = 0; i < resultArray.Length-1; i++)
//            {
//                tempStr = resultArray[i].Substring(match.Index + 1);
//                itemNameRow = layoutArray[i].Split('\t');
//                tempArray = tempStr.Split(new string[] { "\r\n" },StringSplitOptions.None);
//                for (int j = 0; j < tempArray.Length; j++)
//                {
//                    itemResultRow = tempArray[j].Split('\t');
//                    for (int k = 1; k < itemNameRow.Length; k++)
//                    {
//                        string temp = "项目" + itemNameRow[k] + "的值为:" + itemResultRow[k];
//                        Console.WriteLine(temp);
//                        Thread.Sleep(1000);
//                    }
//                }
//            }
//            Console.ReadKey();
//            char CR = ((char)0x0d); //回车符
//            char LF = ((char)0x0a); //换行符
//            string demoStr = @"%$1|A50NF060009C|20170627143948|admin|170627_013|3||||PCT
//MiniTube|PWMBC07C.1|2017.10.12|6.83|||||ng/m||||||G|G||||||||||";
//            if (!demoStr.Contains("$1"))
//                return;

//            int startIndex = demoStr.IndexOf("$1");

//            int endIndex = demoStr.LastIndexOf("||||||||||");
//            string str = demoStr.Substring(startIndex, endIndex - startIndex + 1);
//            var resultStrArray = str.Split('|');
//            var strArray = resultStrArray[4].Split('_');
//            if (strArray.Length > 1)
//            {
//                string tempSid = strArray[1];
//                while (tempSid.StartsWith("0"))
//                {
//                    tempSid = tempSid.TrimStart('0');
//                }
//            }

//            string tempcode = resultStrArray[9].Replace("\r\n", "");


//            Console.ReadKey();

            //string strHelloWord= @"hello word";
            //string[] strlist = strHelloWord.Split(new string[] {"llo"},StringSplitOptions.RemoveEmptyEntries);
            //foreach (var s in strlist)
            //{
            //    Console.WriteLine(s);
            //}
            //string str = string.Empty;
            //int charInt;
            //do
            //{
            //    charInt = Console.Read();
            //    str += ((char) charInt).ToString();
            //} while (charInt != 13);
            //str.Trim('\t');
            //string temp;
            //int value = 0;
            //string result = string.Empty;
            //for (int i = 1; i < str.Length+1; i++)
            //{
            //    if (i % 2 == 0&&i>0)
            //    {
            //        temp = str.Substring(i-2, 2);
            //        value = int.Parse(temp);
            //        result += ((char) value).ToString();
            //    }
            //}
            //Console.WriteLine(result);
            Hashtable ht = new Hashtable();
            ht.Add("insertName","测试调用服务工具");
            XmlDocument doc = WebServiceCaller.QueryPostWebService("http://localhost:8099/GetName.asmx", "GetInsert", ht);
            XmlDocument doc2 = WebServiceCaller.QuerySoapWebService("http://localhost:8099/GetName.asmx", "GetInsert", ht);
            Console.WriteLine(doc.InnerXml);
            Console.WriteLine(doc2.InnerXml);
            Console.ReadKey();
        }
    }
}
