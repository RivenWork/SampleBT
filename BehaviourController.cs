using System.IO;
using System.Xml;

namespace BehaviourTree
{
    /// <summary>
    /// 行为管理器
    /// </summary>
    public class BehaviourController
    {
        XmlReader xReader;
        Stream xml;

        int currentSequenceDepth = 0;

        public BehaviourController(UnityEngine.TextAsset text)
        {
            xml = new MemoryStream(text.bytes);
            if (xml == null)
            {
                throw new System.Exception();
            }
        }

        public bool Update()
        {
            xml.Position = 0;
            currentSequenceDepth = 0;
            xReader = XmlReader.Create(xml);
            return CheckNode(xReader);
        } 

        /// <summary>
        /// 遍历节点
        /// </summary>
        /// <param name="xReader"></param>
        /// <returns></returns>
        bool CheckNode(XmlReader xReader)
        {
            if (xReader.ReadToFollowing("Node"))
            {
                if (xReader.MoveToNextAttribute())
                {
                    if (currentSequenceDepth != 0)
                    {
                        if (xReader.Depth > currentSequenceDepth)
                        {
                            return CheckNode(xReader);
                        }
                    }

                    if (xReader.Value == "Sequence")
                    {
                        var num = 0;
                        var result = true;
                        if (xReader.MoveToNextAttribute())
                        {
                            num = int.Parse(xReader.Value);
                        }
                        else
                        {
                            throw new System.Exception();
                        }
                        for (int i = 0; i < num; i++)
                        {
                            if (!result)
                            {
                                currentSequenceDepth = xReader.Depth - 1;
                                return false;
                            }
                            result = CheckNode(xReader);
                        }
                        return result;
                    }
                    else if (xReader.Value == "Selector")
                    {
                        var num = 0;
                        var result = false;
                        if (xReader.MoveToNextAttribute())
                        {
                            num = int.Parse(xReader.Value);
                        }
                        else
                        {
                            throw new System.Exception();
                        }
                        for (int i = 0; i < num; i++)
                        {
                            if (result) return true;
                            result = CheckNode(xReader);
                        }
                        return result;
                    }
                    else if (xReader.Value == "Condition")
                    {
                        if (xReader.MoveToNextAttribute())
                        {
                            return CheckCondition(int.Parse(xReader.Value));
                        }
                        else
                        {
                            throw new System.Exception();
                        }
                    }
                    else if (xReader.Value == "Action")
                    {
                        if (xReader.MoveToNextAttribute())
                        {
                            return ExecuteAction(int.Parse(xReader.Value));
                        }
                        else
                        {
                            throw new System.Exception();
                        }
                    }
                    else
                    {
                        throw new System.Exception();
                    }
                }
                else
                {
                    throw new System.Exception();
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 处理条件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual bool CheckCondition(int id)
        {
            return true;
        }

        /// <summary>
        /// 处理功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual bool ExecuteAction(int id)
        {
            return true;
        }
    }
}
