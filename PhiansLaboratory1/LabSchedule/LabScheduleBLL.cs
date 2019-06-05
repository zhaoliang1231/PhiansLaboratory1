using LabSchedule.api;
using LabSchedule.bean;
using LabSchedule.impl;
using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using System;
using System.Collections.Generic;

namespace ScheduleClass
{
    public static class LabScheduleBLL
    {
        public static Random Rand = new Random((unchecked((int)DateTime.Now.Ticks)));

        static readonly IScheduleDAL Dal = DALFactory.GetSchedule();

        /// <summary>
        /// 生成排程
        /// </summary>
        /// <param name="mtrNo">mtr编号</param>
        /// <returns></returns>
        public static bool GenerateSchedule(string mtrNo)
        {

            // 填充项目描述
            LabScheduleProject project = new LabScheduleProject
            {
                Calendar = new LabCalendar(8, null), // 标准工时（8小时）
                Clamps = GetClamps(mtrNo), // 夹具
                Devices = GetDevices(mtrNo), // 设备
                Controllers = GetControllers(mtrNo),// 控制器
                Samples = GetSamples(mtrNo) // 检测样本list

            };
          
            // 任务
            FormulatedSchedules formulateds = new FormulatedSchedules();
            formulateds.recover = true;//已排程且尚未执行的日程是否重排
            formulateds.schedules = new List<Schedule>();


            //实例化排程计算器
            IScheduleCalculator calculator = calculator = new ScheduleCalculator(project);
            //计算排程
            ScheduleResult result = calculator.calc();


            //根据排程结果做相应处理
            if (0 == result.resultCode)
            {
                //排程成功
                //存储
                List<TB_Schedule> schedules = new List<TB_Schedule>(); 
                foreach (var schedule in result.schedules)
                {
                    TB_Schedule tBSchedule = new TB_Schedule
                    {
                        ScheduleId = new Guid(schedule.Id),
                        BeginTime = schedule.StartDate.ToString(),
                        EndTime = schedule.FinishDate.ToString(),
                        TaskId = new Guid(schedule.SampleJob.Id),
                        FixtureCode = schedule.Clamp.Id.ToString(),
                        EquipmentNum = schedule.Device.Id.ToString(),
                        MotorNum = schedule.Sample.Id.ToString(),
                        CreateTime = DateTime.Now.ToString()
                    };
                    List<TB_ScheduleControllerOut> controllerOuts = new List<TB_ScheduleControllerOut>();
                    foreach (var scheduleController in schedule.Controllers)
                    {
                        TB_ScheduleControllerOut scheduleControllerOut =
                            new TB_ScheduleControllerOut
                            {
                                ScheduleId = new Guid(schedule.Id),
                                ControllerNum = scheduleController.Id
                            };
                        controllerOuts.Add(scheduleControllerOut);
                    }
                    tBSchedule.Controllers = controllerOuts;

                    schedules.Add(tBSchedule);
                }

                bool flag = Dal.AddSchedule(schedules);

                return flag;
            }
            else
            {
                //排程失败
                return false;
            }

        }
        /// <summary>
        /// 获取设备 需要可以添加参数进行筛选
        /// </summary>
        /// <returns></returns>
        public static IList<RDevice> GetDevices(string mtrNo)
        {
            List<TB_ScheduleEquipment> equipmentList = Dal.GetEquipment(mtrNo);
            IList<RDevice> devices = new List<RDevice>();
            foreach (TB_ScheduleEquipment item in equipmentList)
            {
                RDevice device = new RDevice
                {
                    capacity =(float)item.Capacity,
                    Id = item.ScheduleEquipmentNum,
                    Type = item.EquipmentType,
                    Name = "Test",
                    Status = item.ScheduleState.ToString()
                };
                devices.Add(device);
            }
            return devices;
        }

        /// <summary>
        /// 获取控制器 需要可以添加参数进行筛选
        /// </summary>
        /// <returns></returns>
        public static IList<RController> GetControllers(string mtrNo)
        {
            List<TB_Controller> controllerList = Dal.GetTControllers(mtrNo);
            IList<RController> controllers = new List<RController>();
            //填写内容到控制器集合
            foreach (TB_Controller t in controllerList)
            {
                RController controller = new RController
                {
                    Id = t.ControllerNum,
                    Type = t.ControllerType,
                    Name = "Test",
                    Status = t.Status
                };
                controllers.Add(controller);
            }
            return controllers;
        }

        /// <summary>
        /// 获取夹具 需要可以添加参数进行筛选
        /// </summary>
        /// <returns></returns>
        public static IList<RClamp> GetClamps(string mtrNo)
        {
            List<TB_Fixture> fixtureList = Dal.GetFixture(mtrNo);

            IList<RClamp> clamps = new List<RClamp>();

            //填写内容到夹具集合
            foreach (TB_Fixture t in fixtureList)
            {
                RClamp clamp = new RClamp
                {
                    Id = t.FixtureCode,
                    Type = t.FixtureType ?? "",
                    Name = "Test",
                    Status = t.Status.ToString()
                };
                //填夹具主键 夹具代号
                clamps.Add(clamp);
            }
            return clamps;
        }

        /// <summary>
        /// 获取排程单元
        /// </summary>
        /// <returns></returns>
        public static IList<Schedule> GetSchedules()
        {
            List<Schedule> schedules = new List<Schedule>();
            List<TB_Schedule> tBSchedules = Dal.GetSchedules();
            foreach (var tbSchedule in tBSchedules)
            {
                
            }
            return schedules;
        }


    }
}
