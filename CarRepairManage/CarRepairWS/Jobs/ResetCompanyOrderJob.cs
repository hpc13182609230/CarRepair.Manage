using Quartz;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarRepairWS.Jobs
{
    [DisallowConcurrentExecution]//告诉Quartz不要并发地执行同一个job定义（这里指特定的job类）的多个实例 该限制是针对JobDetail的
    [PersistJobDataAfterExecution]//告诉Quartz在成功执行了job类的execute方法后（没有发生任何异常），更新JobDetail中JobDataMap的数据，使得该job（即JobDetail）在下一次执行的时候，JobDataMap中是更新后的数据，而不是更新前的旧数据 该限制是针对JobDetail的
    public class ResetCompanyOrderJob : IJob
    {
        PartsCompanyService _PartsCompanyService = new PartsCompanyService();

        public void Execute(IJobExecutionContext context)
        {
            LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "log", "ResetCompanyOrderJob start = " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "\r\n");
            ResetCompanyOrder();
        }

        public void ResetCompanyOrder()
        {
            try
            {
                string msg = _PartsCompanyService.ResetCompanyOrder("");
                LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "log", "ResetCompanyOrderJob end = " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "\r\n");
                LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "log", "ResetCompanyOrderJob msg = " + msg + "\r\n");
            }
            catch (Exception ex)
            {
                LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "log", "ResetCompanyOrderJob Exception = " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "\r\n");
                LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "log", "ResetCompanyOrderJob ex = " + ex.Message + "\r\n");
            }
        }

    }
}
