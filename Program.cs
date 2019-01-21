using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NBitcoin;

namespace WasabiRealFeeCalc
{
    internal class Program
    {
        private static readonly string[] txs = new[]{
            "4b06484698e032b9a1f3ccb3b44f60f0a95d27e6638e35fda818116e4fc14985",
            "1387ebeb1797c16f47ab30319be95ceace13c2fb6e0302174ab3d002edcd8cfc",
            "3a8adc4057ab6cafa02fd1fb3b3d7e8411d79c24f0577da5cdb2a84f9350860e",
            "55caf3edf9379f03fe714472a6a9fde2b2d2ced5ea68c8b5ab1b8a7b20be641f",
            "35c0ca79c3d8eebfc718f26f8452a7535ccd589bdfe54eb3405b0f751bedbb58",
            "2df24302244bdb0644cbe741a8c8b2ef3fa7f3fc375b2650ddf93dd9061c871a",
            "9b973b9b1d9cfd719bced68e66ebe92a74566f9449dc6192bb36a77932709362",
            "6ae977738e77222a994db9a4ac914d8d2ebb234797fc07c4d3ec3b39733dceac",
            "a8781bbcc6cb279fa1bec0050ec038d79c4fc2365cfc910377e5ba13f1e90dc4",
            "4aa4d4205c8d6bf7494fd650c923a135afa7327b14e997f8bf151e4ef04dfd1f",
            "7123386071bd45ad2099e3fc9f419809ef03b2f5a7144366c92ae2b6c04a2181",
            "34c6b1e882aa693f316bc14e286ef713caeca16af23871a49c5449a97c3bf818",
            "663079747844bf887a7347fab67beffa127144d142e6576e580c484c25fa2729",
            "077741ec4d1b36c7b2abd92cc5e944491f8d72e7fbd5996a08694003df92f514",
            "b34fcd090a35cb5b8a9d7eed7409d8c17e91f6777d5ac2be21461c4bb125dfc2",
            "e3e77bf31b281af8731d91c5356dbc0f6de49a4e0c85aa91d421315835951bcd",
            "5a84de59b01db5a084c64a95f5b657a96c1fa452b49ba16e7da870dc643ee56b",
            "ec3aabe5ae76d614e796effcec535c58fb08932e048f2210572a3cdc1539ddad",
            "9395b7c4473eb906d9bd6e317d9b57c1d9e12ebc0d52f63f2f455e09fdacb7e8",
            "96c932865a03d7db4a40e09210e0ff36ea7ec65af6e0e7145edfa196590ee29c",
            "90b74eff991417453f70eb79f649213de8821c5a7de5294dfae4c6d90349d12c",
            "ce17bb637a335f27d83cf876f82ef1820c92f81af9cfc1572687de6bae72bf19",
            "870f417d2d9cc2ea468478ea567b119fed14586dd1589ef5a568fa11bd62b495",
            "91ab234adbeecc9ea81227c3235e3d065bb9f3be12e74a9a6bdb6980a7ab375c",
            "01ef2d6a467e3ae748f7f465c44ac252072eddefe3ec06df41a5a90df8fd41dd",
            "ef4448a7d47c193e201b4cae60e0c03fad55b6787ea546cca56a0405ef443ba6",
            "3c2839dbddf1690778e80588855ef813a85f7af0cd8f082476bf9f4ee9899a13",
            "9e451cf444c9adf7cd8b599778008a5fbd56d3f042f079aa9828ba3a9f2705b4",
            "abfc2421b858841f28ecd950d91845acc35376fd400f65959d6230f44e7e39ac",
            //"175388de0864b4d7d81f27a9a5ef7e347aea7d28988b8ff14d09fe4cbb1639cc", BUG
            "7c96fe4c008019b5498b759b4e9a74c24b87008ec214a002ac5edeff4add133a",
            "e2629e490f7340023590dbb946df13ce00e44c8277d08f29ea1fe4c40f32e12a",
            "99d293d97182aff1398a790837e35bbf9c3cece57bc0a566bd7cc44a62137a23",
            "4f7cd454b44a767c8fce0fe591d1238356ae7d71221ec5b9146f70396debf82d",
            "bebe1afeaeefabca8f2d4fcbdeb8d30522bab86d0b31383b4878188920d13c39",
            "a24b3954308861675c21bffa01d06027994c2c42e9bc392a3e6486bdf5eff77d",
            "c07ffc68d1f4db9a612a23c3122e22ea9eacfafd1ae33ceb4fee1e23e6036af4",
            "d252f3346021a0f0949df56701afabf1e08b01a292a6c7ae91a5ec5c0c88fbfc",
            "38e01fee1c753a1e76d10dd28a54d310615ad2aac3aa475d95eef5c80b958a38",
            "2db8e2d58a95dc4ab5c6bb43ff1dcae8502ef60e7bca6dcba76fac6930f4d9e7",
            "d3bb04b512a45cf1fbbf7f8ff3004d0a13c4f398c40bca0b131611c21eeaf8c4",
            "74b71bb3a64aa6800dc18f707c12fcee2dc03abea1483551383b1959664be33a",
            "34aaf80d91cf17fdfb17b92b7bc70579ef1fbc5c5e2d56ea8eb435b1777d7433",
            "a2ccb37e1877ee95c3e8a0c99604356fb2659070a4a42dc5c44c61a0973e1e37",
            "2fa1ba148e53973627a04b83506bc6feca405ddca80ba8c6d08f712c3f631195",
            "0f663395b0e1f29e4ce0dd766b7c14ea04afb0b36b073f4f9c82d775200e84a3",
            "cb45431a3264b20afe88e468a21e2ff1431d0a8148e1c46bb9b40547fd3897a9",
            "7e5898fe0fc3e4ee3a70da169a529e1eb5147eb646eae04ce594dbbd3936b9de",
            "904fdcbb10916b92762fdef39af933c029eddd9474ab20188a936a74389ac8fc",
            "432b5e77cecdae576fa4035d881eec3a6e145ec363ea5ed0e6702b5aa905d646",
            "669e89b426395510fe8554002f2877b6f6b723614e5ef8bf513679aa7e4b6b27",
            "1769af39f4e785e764f2266b61b646b58bfc041b79087ee5ef148d5027985703",
            "2342b910f770da6513ed20d04fdda9f4be1d1584409d67a19826c10797cb907e",
            "8c2770884a870529a72cbf01986c4cd53e1bb915856a3c391950d137de56c2f3",
            "cab7d64407478024aaf90eb8863be68579a5ffa41f0b1671e59f960b22f11234",
            "ba99bb7b248a0bc1c5273ce0b45273cf3a154d8a1cfd708d18c4bc18b89f156c",
            "ff58f93f8ddab635f15f3fa48ffa46880e6e05ebe97f9f4ffd4c1ddbe5b79a39",
            "f17cdb589684f4f457a6da43e6aae621eb41a730cc5a0ca943df2f6a4cb42ab4",
            "d5b702b627dba23a070038d95c40f202025dad8b1c9bf9f75d8ece3a4d85cfc9",
            "428641254c2e47cd29f500f39ea45bf495195d736eeb2c65000efbbdbfc3121e",
            "869fe7f0ba6381adf06f73da8485a916a5d79dd452c972a21939364ae70281ab",
            "204ff90972b6e2396aef63c52c131e3afc43efd5e9fd4eca81dbdaf94ee62b70",
            "5f04ef49a6d1c46116ce78b5f188a4821205b755773d31c35997a6a0dbc2c454",
            "f04359b0e27c53beeba58256c3c54c7d3189974e8f968051b98b52656eaca2eb",
            "89519b516a40dcc89ac6a9e0c7906b34664c9f32fd6a1feadc98a05eb2a24263",
            "6c1ac6ce46900582d913a80656c8129e56347c4d8837151c07d778336ee0c951",
            "961921a670f781645a3f23a204fa170190aecd69df73180a36f478e478cb26f5",
            "540f64fec6ba471273ed38fd729563be19d20385e8667d21a3dc7429021a78f5",
            "b54b202b01df4e09aa5e02f0adace80633db0e62428c8fa891dd23bd9fa27755",
            "8f91c8e306e3e947552fc156673b9174789083e809d18c74ba8b254355d84144",
            "3be9ba29551d0ee248705ae8f12ddea29a7db2176ea89e8170153d32e34b41c0",
            "222c3add4604679608657d96256631517c3c251de82642729fe9d037ba5afd7a",
            "0807912829d537f45117acc50a1810bf00641b3766c1ccdc8807c058925a4ef4",
            "166fbdee82eb14cf3e2988f82567b8b09a4b6fede343121bbec0d5941536edcc",
            "0f36597744b26c6d59094496ed7e327688280e78acf12be8982392aa709d86a3",
            "66e36f570d8181319e9e02a02b084177f55b6093b0816828b237931d94560cb8",
            "c379fb36ca533d37bb5f40e8ed1818f5d7287022257453fe0d097670b5ccaa9b",
            "b1348e5b3cc1d2edeb41efcda1de0f497b8a4f461a3d0287cfc6278071460e11",
            "92979465299ecdd7f4a922d305b1a10a424c53a6cefc4caecfe616a72cc19f36",
            "a15739083dd67818f456dfc1ea84cc2fb9084fc2a2f3647ab24d985be0031822",
            "dd4582401644e07db815429c6a437b367bd40407b273019aaa9c560f47d20aa2",
            "3bca553520c5a0a2fe130c820ae28a7bae3334189c738c41f2f1a7e7d4e06cf9",
            "2033de1c1cb60a7bf99e369e79c50b319a5301eaa665c7bb5c5e9097be5aee25",
            "8e6c20efcc7e2a17b50c7b4382453d2dd34888ef94bd24d8cfc826b2779d82db",
            "710bf1e0bd4ffde6b530b970f1342c9779218c0d9483b8f1d26a7b2ef15c60c2",
            "1f17c7047dfb817e355ced00e4736860d0c2244f7e9a25ee4dfcc293a01ee564",
            "25335b5e87e6e3497d563f9662d33352c932364def1a4f22bf3010ddaad62e8f",
            "336cb1eccda13ce9af9ea778db83845e72057309e96bcfa39f164bd3bbca4481",
            "fffcf9a98ba2363586f3495c3b5c8e3623e29625e81c60e796dc228b41a0876a",
            "a9f7f1e72c43ad8d234a111b86126f32db3c8a39575efc20d127dd5cd237a060",
            "c1a5c5fcf6160eb0248f0063c55870f66752debccbe0f5a5d381f1c96d367423",
            "5f0a1b1cf33850025965def00349cdacbef166218ca5c45496f22a658fd6f015",
            "55e38db2eb87deb0b707ea15685f1f51478fd5e5629088d21e191107683081f6",
            "240756a63536b2aa6c10c38efc7dc4b55fde392ea72bfef1e50324a95466d403",
            "da9fc90fda6dc2fc126b9bc4b61cfd65eae7a2643f303ef157d8c3a0b05813e9",
            "0e17c0f7826e88d178cd1ab34bb2f395d790fc2704be82bfe21fe075df67ec09",
            "23cdc1b9801d0586de0e26ae72e19665d21c445ec5813e2077a35787c966e9f0",
            "650a841104c133e6e106a524f64d08d6de8d1b064acf804fe6f76f014505807a",
            "faad706248fa00282f12a86e056ea3d45285d6a90c44f42c82ba0c062fa6dbdb",
            "72229da92de6d8bba11141804297a14ae4c489f991de18c879c48b26472adb96"
        };

        private static readonly BitcoinWitPubKeyAddress MainNetCoordinatorAddress = new BitcoinWitPubKeyAddress("bc1qs604c7jv6amk4cxqlnvuxv26hv3e48cds4m0ew", Network.Main);

        private static async Task Main(string[] args)
        {
            var total = Money.Zero;
            var totalFees = Money.Zero;
            foreach (var txid in txs)
            {
                var tx = await GetTransactionByIdAsync(txid);

                // Make sure the transaction pays more or less accurate coorditaor fee
                var coordinatorFeeParts = tx.Outputs
                    .GroupBy(x => x.Value)
                    .ToDictionary(x => x.Key, x => x.Count())
                    .Select(x => new { Denomination = x.Key, AnonymitySet = x.Value, Fee = CalculateCoordinatorFee(x.Key, x.Value) })
                    .Where(x => x.AnonymitySet > 1);

                var coordinatorFeeSatoshis = coordinatorFeeParts.Sum(x => (long)x.Fee);

                var fastEstimatiedFee = new Money(coordinatorFeeSatoshis);

                var coordinatorFeeOutput = tx.Outputs.SingleOrDefault(x => x.ScriptPubKey == MainNetCoordinatorAddress.ScriptPubKey);
                var paidCoordinatorFee = coordinatorFeeOutput == null ? Money.Zero : coordinatorFeeOutput.Value;

                var error = fastEstimatiedFee - paidCoordinatorFee;
                var errorPerc = coordinatorFeeSatoshis == 0 ? "NOPAY" : (error.Satoshi / (decimal)coordinatorFeeSatoshis * 100m).ToString("00.00");

                total += error;
                totalFees += paidCoordinatorFee;
                var errorDecimal = error.ToDecimal(MoneyUnit.BTC).ToString("+0.0000000;-0.0000000");
                Console.WriteLine($"{txid}:  Estimated: {fastEstimatiedFee}  Paid: {paidCoordinatorFee}  Diff: {errorDecimal} ({errorPerc}%)   Accumulated: {total}");
            }

            Console.WriteLine($"Total error: {total} btc  --> approx: {total * 3600} USD");
            Console.WriteLine($"Total fees: {totalFees} btc  --> approx: {totalFees * 3600} USD");
            Console.WriteLine($"Error percentage: {(int)(total / (totalFees / 100))}%");
        }

        private static decimal CalculateCoordinatorFee(Money denomination, int anonymitySet)
        {
            var coordinatorFeePerAlice = denomination.Percentage(anonymitySet * anonymitySet * 0.003m);
            return coordinatorFeePerAlice.Satoshi;
        }

        private static async Task<Transaction> GetTransactionByIdAsync(string txid)
        {
            string txHex;
            var cache = Path.Combine("Cache", txid);
            if (File.Exists(cache))
            {
                txHex = File.ReadAllText(cache);
            }
            else
            {
                txHex = await GetBlockchainInfoAsync($"/tx/{txid}?format=hex");
                File.WriteAllText(cache, txHex);
            }
            return Transaction.Parse(txHex, Network.Main);
        }

        private static async Task<string> GetBlockchainInfoAsync(string path)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://blockchain.info");

                using (var response = await httpClient.GetAsync(path, HttpCompletionOption.ResponseContentRead))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new HttpRequestException(response.StatusCode.ToString());
                    string responseString = await response.Content.ReadAsStringAsync();
                    return responseString;
                }
            }
        }
    }

    internal static class NBitcoinExtensions
    {
        public static Money Percentage(this Money me, decimal perc)
        {
            return Money.Satoshis(me.Satoshi * perc * 0.01m);
        }
    }
}
