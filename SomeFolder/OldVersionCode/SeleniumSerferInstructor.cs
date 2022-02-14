/*using OpenQA.Selenium;
using Mouse_Hunter.ScenarioWorkers;
using Mouse_Hunter.ScenarioWorkers.CompositeWorkers;
using Mouse_Hunter.ScenarioWorkers.RamblerRegistrators;
using Mouse_Hunter.ScenarioWorkers.WhRegistrators;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Mouse_Hunter
{
    public class SeleniumSerferInstructor
    {
        private IWebDriver Browser { get; set; }
        public SeleniumSerferInstructor(IWebDriver browser) => Browser = browser;


        public void Quit() => Browser.Quit();

        public void RegistrateOnHappyMin(IProgress<string> progress) =>
            new HappyMinRegistrator(Browser).Execute(progress);

        public void WalkOnStubHub(IProgress<string> progress) =>
            new StubhubWalker(Browser).Execute(progress);

        public void WalkOnWilliamHill(IProgress<string> progress) =>
            new WilliamHillWalker(Browser).Execute(progress);
*//*
        public void RegisterOnWilliamHill(IProgress<string> progress) =>
            new WhClickRegistrator1366_768(Browser).Execute(progress);*//*

        public void ParseUaData(IProgress<string> progress) =>
            new UaDataParser(Browser).Execute(progress);
        public void ParseRusData(IProgress<string> progress) =>
            new RusDataParser(Browser).Execute(progress);
    }
}
*/