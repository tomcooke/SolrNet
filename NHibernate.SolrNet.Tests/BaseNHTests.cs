﻿#region license
// Copyright (c) 2007-2009 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System.Collections.Generic;
using MbUnit.Framework;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Rhino.Mocks;
using SolrNet;

namespace NHibernate.SolrNet.Tests {
    public class BaseNHTests {

        protected ISessionFactory sessionFactory;
        protected ISolrOperations<Entity> mockSolr;

        [SetUp]
        public void FixtureSetup() {
            var nhConfig = new Configuration {
                Properties = new Dictionary<string, string> {
                    {Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider"},
                    {Environment.ConnectionDriver, "NHibernate.Driver.SQLite20Driver"},
                    {Environment.Dialect, "NHibernate.Dialect.SQLiteDialect"},
                    {Environment.ConnectionString, "Data Source=test.db;Version=3;New=True;"},
                }
            };
            nhConfig.Register(typeof (Entity));
            mockSolr = MockRepository.GenerateMock<ISolrOperations<Entity>>();
            nhConfig.SetListener(new SolrNetListener<Entity>(mockSolr));
            new SchemaExport(nhConfig).Execute(false, true, false, false);
            sessionFactory = nhConfig.BuildSessionFactory();
        }

        [TearDown]
        public void FixtureTeardown() {
            sessionFactory.Dispose();
        }
    }
}