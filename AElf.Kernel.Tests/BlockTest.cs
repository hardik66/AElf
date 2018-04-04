﻿using System.Threading.Tasks;
using AElf.Kernel.KernelAccount;
using Xunit;
using Xunit.Frameworks.Autofac;

namespace AElf.Kernel.Tests
{
    [UseAutofacTestFramework]
    public class BlockTest
    {
        private IBlockManager _blockManager;
        private ISmartContractZero _smartContractZero;
        private ChainTest _chainTest;


        public BlockTest(IBlockManager blockManager, ISmartContractZero smartContractZero, ChainTest chainTest)
        {
            _blockManager = blockManager;
            _smartContractZero = smartContractZero;
            _chainTest = chainTest;
        }

     

       [Fact]
        public void GenesisBlockBuilderTest()
        {
            var builder = new GenesisBlockBuilder().Build(_smartContractZero);
            var genesisBlock = builder.Block;
            var tx = builder.Tx;
            Assert.NotNull(genesisBlock);
            Assert.Equal(genesisBlock.Header.PreviousHash, Hash.Zero);
            Assert.NotNull(tx);
        }

        [Fact]
        public async Task BlockManagerTest()
        {
            var builder = new GenesisBlockBuilder().Build(_smartContractZero);
            var genesisBlock = builder.Block;

            await _blockManager.AddBlockAsync(genesisBlock);
            var blockHeader = await _blockManager.GetBlockHeaderAsync(genesisBlock.GetHash());

            var block = new Block(genesisBlock.GetHash());

            var chain = await _chainTest.CreateChain();
            await _chainTest.AppendBlock(chain, block);
            Assert.Equal(blockHeader, genesisBlock.Header);
        }
        
        
    }
}