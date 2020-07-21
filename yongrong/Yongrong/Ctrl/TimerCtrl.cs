﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BaseS.File.Log;
using Yongrong.Srvc.BaseInfo;

namespace Yongrong.Ctrl
{
    class TimerCtrl : BaseCtrl
    {
        /// <summary>
        /// 定时器锁对象
        /// </summary>
        static readonly object hourTimeLocker = new object();

        /// <summary>
        /// 定时器锁对象
        /// </summary>
        static readonly object minuteTimeLocker = new object();

        /// <summary>
        /// 定时器锁对象
        /// </summary>
        static readonly object dayTimeLocker = new object();

        /// <summary>
        /// 按秒
        /// </summary>
        static readonly Timer second = new Timer(AllTask);

        /// <summary>
        /// 最后一次分钟调用是时间
        /// </summary>
        static DateTime lastTime = DateTime.Now;

        public static Action<object> SecondAction { get; set; }
        public static Action<object> MinuteAction { get; set; }
        public static Action<object> HourAction { get; set; }
        public static Action<object> DayAction { get; set; }

        static TimerCtrl()
        {
            DayAction += OrderGoodsSrvc.TaskDay;
        }

        /// <summary>
        /// 开启定时器
        /// </summary>
        public static void Start()
        {
            second.Change(Sys.TimeTaskDueTime, Sys.TimeTaskPeriod);

            
        }

        /// <summary>
        /// 关闭定时器
        /// </summary>
        public static void Stop()
        {
            second.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// 秒调用定时器
        /// </summary>
        /// <param name="o"></param>
        private static void AllTask(object o)
        {
            try
            {
                bool isMinute = DateTime.Now.Minute != lastTime.Minute;
                bool isHour = DateTime.Now.Hour != lastTime.Hour;
                bool isDay = DateTime.Now.Day != lastTime.Day;

                lastTime = DateTime.Now;

                try
                {
                    SecondAction?.Invoke(o);
                }
                catch (Exception e)
                {
                    $"SecondAction Err:{e.Message} {e.StackTrace}".Warn();
                }

                if (isMinute)
                {
                    if (Monitor.TryEnter(minuteTimeLocker))
                    {
                        try
                        {
                            MinuteAction?.Invoke(o);
                        }
                        catch (Exception e)
                        {
                            $"MinuteAction Err:{e.Message} {e.StackTrace}".Warn();
                        }
                        finally
                        {
                            Monitor.Exit(minuteTimeLocker);
                        }
                    }
                    else
                    {
                        $"定时分钟任务存在问题,上一个任务未结束".Warn();
                    }
                }

                if (isHour)
                {
                    if (Monitor.TryEnter(hourTimeLocker))
                    {
                        try
                        {
                            $"小时任务开始".Debug();
                            HourAction?.Invoke(o);
                            $"小时任务结束".Debug();
                        }
                        catch (Exception e)
                        {
                            $"HourAction Err:{e.Message} {e.StackTrace}".Warn();
                        }
                        finally
                        {
                            Monitor.Exit(hourTimeLocker);
                        }
                    }
                    else
                    {
                        $"定时小时任务存在问题,上一个任务未结束".Warn();
                    }
                }

                if (isDay)
                {
                    if (Monitor.TryEnter(dayTimeLocker))
                    {
                        try
                        {
                            $"天任务开始".Debug();
                            DayAction?.Invoke(o);
                            $"天任务结束".Debug();
                        }
                        catch (Exception e)
                        {
                            $"DayAction Err:{e.Message} {e.StackTrace}".Warn();
                        }
                        finally
                        {
                            Monitor.Exit(dayTimeLocker);
                        }
                    }
                    else
                    {
                        $"定时天任务存在问题,上一个任务未结束".Warn();
                    }
                }
            }
            catch (Exception e)
            {
                $"Exception:{e.Message} {e.StackTrace}".Warn();
            }
        }
    }
}
