using NUnit.Framework;
using Sprache;

namespace ObjectConditions.Tests
{
    [TestFixture]
    public class BoundaryConditions
    {
        [Test]
        public void NestedExpressionsTest()
        {
            const string str = "!(!(true   =   ConfigValue::dvfyBPS4   )   =   ConfigValue::dvfyBPS4   )";

            var ast = new UnaryRelation()
            {
                Expression = new BinaryRelation()
                {
                    Left = new UnaryRelation()
                    {
                        Expression = new BinaryRelation()
                        {
                            Left = new ObjectValue()
                            {
                                Value = "true"
                            },
                            Operator = BinaryOperators.Equality,
                            Right = new TypedObject()
                            {
                                Name = "dvfyBPS4",
                                ObjectType = "ConfigValue"
                            }
                        },
                        Operator = UnaryOperators.Negation
                    },
                    Operator = BinaryOperators.Equality,
                    Right = new TypedObject()
                    {
                        Name = "dvfyBPS4",
                        ObjectType = "ConfigValue"
                    }
                },
                Operator = UnaryOperators.Negation
            };

            var parsed = LanguageGrammar.ParseExpression.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void BinaryRelationTest()
        {
            const string str = "( Yga9SELk < MDJoNveF::XtuJduTe)";

            var ast = new BinaryRelation()
            {
                Left = new ObjectValue()
                {
                    Value = "Yga9SELk"
                },
                Operator = BinaryOperators.LessThan,
                Right = new TypedObject()
                {
                    Name = "XtuJduTe",
                    ObjectType = "MDJoNveF"
                }
            };

            var parsed = LanguageGrammar.ParseExpression.Parse(str);
            Assert.AreEqual(ast, parsed);

            const string str2 = "3O3MDn2I >!   Hmys6L2I::Df4A1pzX";

            var ast2 = new BinaryRelation()
            {
                Left = new ObjectValue()
                {
                    Value = "3O3MDn2I"
                },
                Operator = BinaryOperators.GreaterThan,
                Right = new UnaryRelation()
                {
                    Expression = new TypedObject()
                    {
                        Name = "Df4A1pzX",
                        ObjectType = "Hmys6L2I"
                    },
                    Operator = UnaryOperators.Negation
                }
            };

            var parsed2 = LanguageGrammar.ParseExpression.Parse(str2);
            Assert.AreEqual(ast2, parsed2);
        }

        [Test]
        public void UnaryRelationsNestingTest()
        {
            const string str = "!!VFcsfyxF::V2dnlJGW < 1yjlGOQR";

            var ast = new BinaryRelation()
            {
                Left = new UnaryRelation()
                {
                    Expression = new UnaryRelation()
                    {
                        Expression = new TypedObject()
                        {
                            Name = "V2dnlJGW",
                            ObjectType = "VFcsfyxF"
                        },
                        Operator = UnaryOperators.Negation
                    },
                    Operator = UnaryOperators.Negation
                },
                Operator = BinaryOperators.LessThan,
                Right = new ObjectValue()
                {
                    Value = "1yjlGOQR"
                }
            };

            var parsed = LanguageGrammar.ParseExpression.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void ChildrenTest()
        {
            var ast = new BinaryRelation()
            {
                Left = new TypedObject(),
                Operator = BinaryOperators.LessThan,
                Right = new ObjectValue()
            };

            Assert.AreEqual(2, ast.Children.Count);
            Assert.AreEqual(0, ast.Children[0].Children.Count);
            Assert.AreEqual(0, ast.Children[1].Children.Count);

            ast = new BinaryRelation()
            {
                Left = new BinaryRelation()
                {
                    Left = new ObjectValue()
                    {
                        Value = "value1"
                    },
                    Operator = BinaryOperators.Equality,
                    Right = new TypedObject()
                    {
                        Name = "object1",
                        ObjectType = "type1"
                    }
                },
                Operator = BinaryOperators.Equality,
                Right = new TypedObject()
                {
                    Name = "object2",
                    ObjectType = "type2"
                }
            };

            var flat = Helper.GetFlatTree(ast);
            Assert.AreEqual(5, flat.Count);
        }

        [Test]
        public void EmptyObjectsTest()
        {
            var ast = new BinaryRelation()
            {
                Left = new BinaryRelation()
                {
                    Left = new ObjectValue(),
                    Operator = BinaryOperators.Equality,
                    Right = new TypedObject()
                },
                Operator = BinaryOperators.Equality,
                Right = new TypedObject()
            };

            var flat = Helper.GetFlatTree(ast);
            Assert.AreEqual(5, flat.Count);
        }

        [Test]
        public void UnaryRelationTest()
        {
            const string str = "!(mwrt5xDK)";

            var ast = new UnaryRelation()
            {
                Expression = new ObjectValue()
                {
                    Value = "mwrt5xDK"
                },
                Operator = UnaryOperators.Negation
            };

            var parsed = LanguageGrammar.ParseExpression.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void BinaryRelationWithUnaryRelationTest()
        {
            const string str = "w4oKP4rl::sHjPcCRy Or NotExist(mwrt5xDK)";

            var ast = new BinaryRelation()
            {
                Left = new TypedObject()
                {
                    Name = "sHjPcCRy",
                    ObjectType = "w4oKP4rl"
                },
                Operator = BinaryOperators.Disjunction,
                Right = new UnaryRelation()
                {
                    Operator = UnaryOperators.NotExist,
                    Expression = new ObjectValue()
                    {
                        Value = "mwrt5xDK"
                    }
                }
            };

            var parsed = LanguageGrammar.ParseExpression.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void BasicNestingOrderTest()
        {
            const string str = "xHlqT243::1bRn2sQA < ! NotExist VzN6P0Lb::zSm2eseU > ZesXQQU1 = Exist !(JXXPBsEv)";

            var ast = new BinaryRelation()
            {
                Left = new TypedObject()
                {
                    Name = "1bRn2sQA",
                    ObjectType = "xHlqT243"
                },
                Operator = BinaryOperators.LessThan,
                Right = new BinaryRelation()
                {
                    Left = new UnaryRelation()
                    {
                        Expression = new UnaryRelation()
                        {
                            Expression = new TypedObject()
                            {
                                ObjectType = "VzN6P0Lb",
                                Name = "zSm2eseU"
                            },
                            Operator = UnaryOperators.NotExist
                        },
                        Operator = UnaryOperators.Negation
                    },
                    Operator = BinaryOperators.GreaterThan,
                    Right = new BinaryRelation()
                    {
                        Left = new ObjectValue()
                        {
                            Value = "ZesXQQU1"
                        },
                        Operator = BinaryOperators.Equality,
                        Right = new UnaryRelation()
                        {
                            Expression = new UnaryRelation()
                            {
                                Expression = new ObjectValue()
                                {
                                    Value = "JXXPBsEv",
                                },
                                Operator = UnaryOperators.Negation
                            },
                            Operator = UnaryOperators.Exist
                        }
                    }
                }
            };

            var parsed = LanguageGrammar.ParseExpression.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        [ExpectedException(typeof(ParseException))]
        public void EndOfInputTest()
        {
            const string str = "xHlqT243::1bRn2sQA < ! NotExist VzN6P0Lb::zSm2eseU > ZesXQQU1 = Exist !(JXXPBsEv)))";
            LanguageGrammar.ParseExpression.Parse(str);
        }
    }
}
