using System;
using System.Threading;

public class AlarmClock
{
    public event Action Tick;
    public event Action Alarm;

    private int _seconds;
    private int _alarmTime;

    public AlarmClock(int alarmTime)
    {
        _alarmTime = alarmTime;
        _seconds = 0;
    }

    public void Start()
    {
        while (_seconds <= _alarmTime)
        {
            Thread.Sleep(1000); // 模拟1秒
            _seconds++;

            Tick?.Invoke(); // 触发Tick事件

            if (_seconds == _alarmTime)
            {
                Alarm?.Invoke(); // 触发Alarm事件
            }
        }
    }
}

class Program
{
    static void Main()
    {
        AlarmClock clock = new AlarmClock(5); // 5秒后闹钟响铃

        clock.Tick += () => Console.WriteLine("嘀嗒...");
        clock.Alarm += () => Console.WriteLine("闹钟响了！");

        clock.Start();
    }
}
