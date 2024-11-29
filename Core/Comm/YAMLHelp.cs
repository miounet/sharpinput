using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core.Comm
{
    public class YAMLHelp
    {
        // 所有行
        private readonly string[] lines;
        // 格式化为节点
        private List<Node> nodeList = new List<Node>();
        // 文件所在地址
        private readonly string path;

        public YAMLHelp(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            this.path = path;
            this.lines = File.ReadAllLines(path);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Trim() == "")
                {
                    Console.WriteLine("空白行，行号：" + (i + 1));
                    continue;
                }
                else if (line.Trim().Substring(0, 1) == "#")
                {
                    Console.WriteLine("注释行，行号：" + (i + 1));
                    continue;
                }

                string[] kv = Regex.Split(line, ":", RegexOptions.IgnoreCase);
                _ = findPreSpace(line);
                Node node = new Node();
                node.Space = findPreSpace(line);
                node.Name = kv[0].Trim();

                // 去除前后空白符
                string fline = line.Trim();
                int first = fline.IndexOf(':');
                node.Value = first == fline.Length - 1 ? null : fline.Substring(first + 2, fline.Length - first - 2);
                node.Parent = findParent(node.Space);
                nodeList.Add(node);
            }

            this.formatting();
        }

        // 修改值 允许key为多级 例如：spring.datasource.url
        public void modify(string key, string value)
        {
            Node node = findNodeByKey(key);
            if (node != null)
                node.Value = value;
        }

        // 读取值
        public string read(string key, string value)
        {
            Node node = findNodeByKey(key);
            return node != null ? node.Value : null;
        }

        // 根据key找节点
        public Node findNodeByKey(string key)
        {
            string[] ks = key.Split('.');
            for (int i = 0; i < nodeList.Count; i++)
            {
                Node node = nodeList[i];
                if (node.Name == ks[ks.Length - 1])
                {
                    // 判断父级
                    Node tem = node;
                    // 统计匹配到的次数
                    int count = 1;
                    for (int j = ks.Length - 2; j >= 0; j--)
                    {
                        if (tem.Parent.Name == ks[j])
                        {
                            count++;
                            // 继续检查父级
                            tem = tem.Parent;
                        }
                    }

                    if (count == ks.Length)
                        return node;
                }
            }
            return null;
        }

        // 格式化
        public void formatting()
        {
            // 先找出根节点
            List<Node> parentNode = new List<Node>();
            for (int i = 0; i < nodeList.Count; i++)
            {
                Node node = nodeList[i];
                if (node.Parent == null)
                    parentNode.Add(node);
            }

            List<Node> fNodeList = new List<Node>();
            // 遍历根节点
            for (int i = 0; i < parentNode.Count; i++)
            {
                Node node = parentNode[i];
                fNodeList.Add(node);
                findChildren(node, fNodeList);
            }

            Console.WriteLine("完成");

            // 指针指向格式化后的
            nodeList = fNodeList;
        }

        // 层级
        private int tier = 0;

        // 查找孩子并进行分层
        public void findChildren(Node node, List<Node> fNodeList)
        {
            // 当前层 默认第一级，根在外层进行操作
            tier++;

            for (int i = 0; i < nodeList.Count; i++)
            {
                Node item = nodeList[i];
                if (item.Parent == node)
                {
                    item.Tier = tier;
                    fNodeList.Add(item);
                    findChildren(item, fNodeList);
                }
            }

            // 走出一层
            tier--;
        }

        //查找前缀空格数量
        private int findPreSpace(string str)
        {
            List<char> chars = str.ToList();
            int count = 0;
            foreach (char c in chars)
            {
                if (c == ' ')
                    count++;
                else
                    break;
            }
            return count;
        }

        // 根据缩进找上级
        private Node findParent(int space)
        {
            if (nodeList.Count == 0)
                return null;

            // 倒着找上级
            for (int i = nodeList.Count - 1; i >= 0; i--)
            {
                Node node = nodeList[i];
                if (node.Space < space)
                    return node;
            }

            // 如果没有找到 返回null
            return null;
        }

        // 私有节点类
        public class Node
        {
            // 名称
            public string Name { get; set; }
            // 值
            public string Value { get; set; }
            // 父级
            public Node Parent { get; set; }
            // 前缀空格
            public int Space { get; set; }
            // 所属层级
            public int Tier { get; set; }
        }
    }
}
