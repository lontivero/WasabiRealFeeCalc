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
    class Program
    {
        private static string[] txs = new[]{
            "f17cdb589684f4f457a6da43e6aae621eb41a730cc5a0ca943df2f6a4cb42ab4",
            "ff58f93f8ddab635f15f3fa48ffa46880e6e05ebe97f9f4ffd4c1ddbe5b79a39",
            "ba99bb7b248a0bc1c5273ce0b45273cf3a154d8a1cfd708d18c4bc18b89f156c",
            "cab7d64407478024aaf90eb8863be68579a5ffa41f0b1671e59f960b22f11234",
            "8c2770884a870529a72cbf01986c4cd53e1bb915856a3c391950d137de56c2f3",
            "2342b910f770da6513ed20d04fdda9f4be1d1584409d67a19826c10797cb907e",
            "1769af39f4e785e764f2266b61b646b58bfc041b79087ee5ef148d5027985703",
            "669e89b426395510fe8554002f2877b6f6b723614e5ef8bf513679aa7e4b6b27",
            "432b5e77cecdae576fa4035d881eec3a6e145ec363ea5ed0e6702b5aa905d646",
            "904fdcbb10916b92762fdef39af933c029eddd9474ab20188a936a74389ac8fc",
            "7e5898fe0fc3e4ee3a70da169a529e1eb5147eb646eae04ce594dbbd3936b9de",
            "cb45431a3264b20afe88e468a21e2ff1431d0a8148e1c46bb9b40547fd3897a9",
            "0f663395b0e1f29e4ce0dd766b7c14ea04afb0b36b073f4f9c82d775200e84a3",
            "2fa1ba148e53973627a04b83506bc6feca405ddca80ba8c6d08f712c3f631195",
            "a2ccb37e1877ee95c3e8a0c99604356fb2659070a4a42dc5c44c61a0973e1e37",
            "34aaf80d91cf17fdfb17b92b7bc70579ef1fbc5c5e2d56ea8eb435b1777d7433",
            "74b71bb3a64aa6800dc18f707c12fcee2dc03abea1483551383b1959664be33a",
            "d3bb04b512a45cf1fbbf7f8ff3004d0a13c4f398c40bca0b131611c21eeaf8c4",
            "2db8e2d58a95dc4ab5c6bb43ff1dcae8502ef60e7bca6dcba76fac6930f4d9e7",
            "38e01fee1c753a1e76d10dd28a54d310615ad2aac3aa475d95eef5c80b958a38",
            "d252f3346021a0f0949df56701afabf1e08b01a292a6c7ae91a5ec5c0c88fbfc",
            "c07ffc68d1f4db9a612a23c3122e22ea9eacfafd1ae33ceb4fee1e23e6036af4",
            "a24b3954308861675c21bffa01d06027994c2c42e9bc392a3e6486bdf5eff77d",
            "bebe1afeaeefabca8f2d4fcbdeb8d30522bab86d0b31383b4878188920d13c39",
            "4f7cd454b44a767c8fce0fe591d1238356ae7d71221ec5b9146f70396debf82d",
            "99d293d97182aff1398a790837e35bbf9c3cece57bc0a566bd7cc44a62137a23",
            "e2629e490f7340023590dbb946df13ce00e44c8277d08f29ea1fe4c40f32e12a",
            "7c96fe4c008019b5498b759b4e9a74c24b87008ec214a002ac5edeff4add133a",
            // "175388de0864b4d7d81f27a9a5ef7e347aea7d28988b8ff14d09fe4cbb1639cc", BUG!!!
            "abfc2421b858841f28ecd950d91845acc35376fd400f65959d6230f44e7e39ac",
            "9e451cf444c9adf7cd8b599778008a5fbd56d3f042f079aa9828ba3a9f2705b4",
            "3c2839dbddf1690778e80588855ef813a85f7af0cd8f082476bf9f4ee9899a13",
            "ef4448a7d47c193e201b4cae60e0c03fad55b6787ea546cca56a0405ef443ba6",
            "01ef2d6a467e3ae748f7f465c44ac252072eddefe3ec06df41a5a90df8fd41dd",
            "91ab234adbeecc9ea81227c3235e3d065bb9f3be12e74a9a6bdb6980a7ab375c",
            "870f417d2d9cc2ea468478ea567b119fed14586dd1589ef5a568fa11bd62b495",
            "ce17bb637a335f27d83cf876f82ef1820c92f81af9cfc1572687de6bae72bf19",
            "90b74eff991417453f70eb79f649213de8821c5a7de5294dfae4c6d90349d12c",
            "96c932865a03d7db4a40e09210e0ff36ea7ec65af6e0e7145edfa196590ee29c",
            "9395b7c4473eb906d9bd6e317d9b57c1d9e12ebc0d52f63f2f455e09fdacb7e8",
            "ec3aabe5ae76d614e796effcec535c58fb08932e048f2210572a3cdc1539ddad",
            "5a84de59b01db5a084c64a95f5b657a96c1fa452b49ba16e7da870dc643ee56b",
            "e3e77bf31b281af8731d91c5356dbc0f6de49a4e0c85aa91d421315835951bcd",
            "b34fcd090a35cb5b8a9d7eed7409d8c17e91f6777d5ac2be21461c4bb125dfc2",
            "077741ec4d1b36c7b2abd92cc5e944491f8d72e7fbd5996a08694003df92f514",
            "663079747844bf887a7347fab67beffa127144d142e6576e580c484c25fa2729",
            "34c6b1e882aa693f316bc14e286ef713caeca16af23871a49c5449a97c3bf818",
            "7123386071bd45ad2099e3fc9f419809ef03b2f5a7144366c92ae2b6c04a2181",
            "4aa4d4205c8d6bf7494fd650c923a135afa7327b14e997f8bf151e4ef04dfd1f",
            "a8781bbcc6cb279fa1bec0050ec038d79c4fc2365cfc910377e5ba13f1e90dc4",
            "6ae977738e77222a994db9a4ac914d8d2ebb234797fc07c4d3ec3b39733dceac",
            "9b973b9b1d9cfd719bced68e66ebe92a74566f9449dc6192bb36a77932709362",
            "2df24302244bdb0644cbe741a8c8b2ef3fa7f3fc375b2650ddf93dd9061c871a",
            "35c0ca79c3d8eebfc718f26f8452a7535ccd589bdfe54eb3405b0f751bedbb58",
            "55caf3edf9379f03fe714472a6a9fde2b2d2ced5ea68c8b5ab1b8a7b20be641f",
            "3a8adc4057ab6cafa02fd1fb3b3d7e8411d79c24f0577da5cdb2a84f9350860e",
            "1387ebeb1797c16f47ab30319be95ceace13c2fb6e0302174ab3d002edcd8cfc",
            "4b06484698e032b9a1f3ccb3b44f60f0a95d27e6638e35fda818116e4fc14985",
            "463310c8ab49a07c8f7f03b83859f6b63f08a33bbbada98bd1b1ae4d5dac958a",
            "d76a9dc3b4c1e748542301ff4887b3718614597400dc8c218080e17440b95d0a",
            "89cb70212862217c0edef357fb966d2ba2335fbc26ec4536e234dfcd6cfd6524",
            "59987bc76e85fd1564ac5bc06e40b4f4fd0121a21e798f23c636677e9a767295",
            "7fdcb90c1e5158c8d17dea369846c4d7181fcee9e63dc8824baa52be72b997f0",
            "e810bc6c50eb33dcc774f29ea5db9c7cb2675a116493d85d7c7d18241fe13111",
            "76e94181061b8136820678325aeb9feaefe3edc8d269afae7a77a031c399bf0d",
            "939e0ea623ec059b9b30fc42fe21ee0f32f0e6bcad61c5cfc92661a82d1b62ed",
            "e63e66d711c688de90baf4adf4e99e6a21afc7df2bd3e30cea72d1f7e2535eea",
            "192fcb2bb28d5e85a4a1d55653a52d48e4256ae87a7113758fd1c22124e44735",
            "bb6ebc3f1b22bd1084dce6cda01f4a9f80cf192ab94eecc6235c8b54fa7629e5",
            "9e14e27ebb21583a00628a6b499aef31a1774e04e15b258a79d58077c7799096",
            "da7f0f8c1295ff3f5ad3099ea173aebd863c58c5be9bbf8ce51af739ddf70a3c",
            "0e42b00cb542db8d00d1e4b4673ef1b2db0c33c96c7be7ba9615ca397a61de94",
            "579e16541306fd1b46d7998fcf3a14bbfd7b2c19b73a19e01cd62c74b3c977fb",
            "7e81759850f0fc1a66c01db0b577eb68f5634a69e58e99db19f062f5d6a57171",
            "c9fa103e8b8b26dd86e775a41b10d2eca985a3a9654629bd9d6d17948d54b82a",
            "207e55e4fea08e7ecfb01a69823029968cd3989d89697d4a25549ea15fb16109",
            "4eb197b53760b2cf1240f638f41863fb1b67fa5bc443c34f9805e01bd7b82e8c",
            "cf06bca26e511b87d77f382ef85395c6083061e88b6a45148d8a769e6cd53530",
            "a0daad9064906d29036502a1c13b1abdf71494a3daa9432e7d738bccde5d07eb",
            "79ab1adc88617c6edb426297015439661247cd508faba9e437ea27c1a3aeb065",
            "3573229b2d7fde9f898b6b6f731911d45d1b81ab5bf80b31260547c254741aad",
            "cc9dd33ae13c98749dc26308f17b2db9dba79b70e554386b7e02ae70b5150f90",
            "4dfb63958e43695874609941fd3e3e184c51a4d9e0a73e3c6faa7f67f235c01a",
            "97a86e22b27bd2ffede6e4c118394da06fb1234e343a4e49243f3a4f0a8ef09d",
            "e0648cf19b9b16802fbce0378422da62d9487e92018469d8f25c32a926b92c66",
            "24c2a613b39f014dd261534abc543e32b50d4c1730a6f19a240d38b786b0ba5b",
            "52553e8f0be62646d7b1e9c79d3f89f76843550b4b62095a9f7504186e26ff81",
            "115aed3364e9ce150d4ddb0a0de2f51b75b18c4ef2909f8057680bae22844cef",
            "7a95342af9c64083e86d5fbb420f8becad0620af3fac85718ff40ba7c2527c06",
            "bbd3f6fd1728d4eeab6e8e7c0613e880550196e5586037b8ac56e6c3dbfb6bca",
            "b56422580a085003cc9028b145a719c6a8baa2aa6691dd1cf488e4e52f537b15",
            "3bdce85cfd7186248c1f55ceeb24366d542205a943676c92a36ae67145c14b8e",
            "fcdbd40bc6bbb523f233f40ac4d7d9be9cf961883edc3e581c6598b4098c3890",
            "d473742459f624a06b0701367d5f6df61373408c8433e32d216a59d03afc7fe7",
            "382d08af407a01d4d824b86a3311ec0ecc9cb2d7fa0b96e6499cd33d76d08d62",
            "cf276acd25297a882399ccb803ac0f08077b774ed40eb38b674bcd97893e09d5",
            "01bf9ca282ec631cb51b25234ef9478a166ec10c0a42827409094ae05fe68a13",
            "bee8399f6ee50d78d230a8857741a08b0a8fff0e7dc4e9b412d781b96b345e67",
            "924e775e3af2f151fc39745a036e26f29b3bf2426e582cce7118cccbfbb285ad"
        };

        private static readonly BitcoinWitPubKeyAddress MainNetCoordinatorAddress = new BitcoinWitPubKeyAddress("bc1qs604c7jv6amk4cxqlnvuxv26hv3e48cds4m0ew", Network.Main);

        static async Task Main(string[] args)
        {
            var total = Money.Zero;
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
                var errorPerc =  coordinatorFeeSatoshis == 0 ? "NOPAY" : (error.Satoshi / (decimal)coordinatorFeeSatoshis * 100m).ToString("00.00");

                total += error;
                var errorDecimal = error.ToDecimal(MoneyUnit.BTC).ToString("+0.0000000;-0.0000000");
                Console.WriteLine($"{txid}:  Estimated: {fastEstimatiedFee}  Paid: {paidCoordinatorFee}  Diff: {errorDecimal} ({errorPerc}%)   Accumulated: {total}");
            }

            Console.WriteLine($"Total error: {total} btc  --> approx: {total * 3600} USD");
        }

        private static decimal CalculateCoordinatorFee(Money denomination, int anonymitySet)
        {
            var coordinatorFeePerAlice = denomination.Percentage(anonymitySet * anonymitySet * 0.003m );
            return coordinatorFeePerAlice.Satoshi;
        }

        private static async Task<Transaction> GetTransactionByIdAsync(string txid)
        {
            string txHex;
            var cache = Path.Combine("Cache", txid);
            if(File.Exists(cache))
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

    static class NBitcoinExtensions
    {
		public static Money Percentage(this Money me, decimal perc)
		{
			return Money.Satoshis(me.Satoshi * perc * 0.01m );
		}
    }
}
