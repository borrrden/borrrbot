// 
//  ComponentScheduler.cs
// 
//  This is free and unencumbered software released into the public domain.
//
//  Anyone is free to copy, modify, publish, use, compile, sell, or
//  distribute this software, either in source code form or as a compiled
//  binary, for any purpose, commercial or non-commercial, and by any
//  means.
//
//  In jurisdictions that recognize copyright laws, the author or authors
//  of this software dedicate any and all copyright interest in the
//  software to the public domain. We make this dedication for the benefit
//  of the public at large and to the detriment of our heirs and
//  successors. We intend this dedication to be an overt act of
//  relinquishment in perpetuity of all present and future rights to this
//  software under copyright law.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//  MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//  IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//  OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//  ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//  OTHER DEALINGS IN THE SOFTWARE.
//
//  For more information, please refer to <http://unlicense.org/>
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using borrrbot.API;

using JetBrains.Annotations;

namespace borrrbot.Components
{
    internal sealed class ComponentScheduler
    {
        #region Variables

        [NotNull] private readonly SortedDictionary<DateTimeOffset, IBotComponent> _schedule =
            new SortedDictionary<DateTimeOffset, IBotComponent>();

        [NotNull] private readonly Timer _timer;
        private DateTimeOffset _nextTimerFire;

        #endregion

        #region Constructors

        public ComponentScheduler()
        {
            _timer = new Timer(Tick);
        }

        #endregion

        #region Public Methods

        public void Activate(BotEngine engine)
        {
            foreach (var item in _schedule.Values) {
                item.Activate(engine);
            }
        }

        public void Schedule(IBotComponent component)
        {
            var nextTime = DateTimeOffset.UtcNow + component.Interval;
            _schedule.Add(nextTime, component);
            if (_schedule.Count == 1 || _schedule.FirstOrDefault().Key == nextTime) {
                _timer.Change(component.Interval, TimeSpan.FromMilliseconds(-1));
            }
        }

        public void Unschedule(IBotComponent component)
        {
            var entry = _schedule.FirstOrDefault(x => x.Value == component);
            if (entry.Value == null) {
                return;
            }

            _schedule.Remove(entry.Key);
        }

        #endregion

        #region Private Methods

        private void Tick(object arg)
        {
            var first = _schedule.FirstOrDefault();
            if (first.Value == null) {
                return;
            }

            _schedule.Remove(first.Key);
            first.Value.Perform();
            _schedule.Add(DateTimeOffset.UtcNow + first.Value.Interval, first.Value);
            var newFirst = _schedule.FirstOrDefault();
            if (newFirst.Value == null) {
                return;
            }

            var nextInterval = newFirst.Key - DateTimeOffset.UtcNow;
            if (nextInterval <= TimeSpan.Zero) {
                nextInterval = TimeSpan.FromMilliseconds(-1);
            }

            _timer.Change(nextInterval, TimeSpan.FromMilliseconds(-1));
        }

        #endregion
    }
}
