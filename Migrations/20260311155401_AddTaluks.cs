using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PMS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddTaluks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Taluks",
                columns: new[] { "TalukId", "DistrictId", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, 1, true, "Anekal" },
                    { 2, 1, true, "Bengaluru East" },
                    { 3, 1, true, "Bengaluru North" },
                    { 4, 1, true, "Bengaluru South" },
                    { 5, 1, true, "Yeshwanthapura" },
                    { 6, 2, true, "Devanahalli" },
                    { 7, 2, true, "Doddaballapur" },
                    { 8, 2, true, "Hosakote" },
                    { 9, 2, true, "Nelamangala" },
                    { 10, 3, true, "H.D. Kote" },
                    { 11, 3, true, "Hunsur" },
                    { 12, 3, true, "K.R. Nagar" },
                    { 13, 3, true, "Mysuru" },
                    { 14, 3, true, "Nanjangud" },
                    { 15, 3, true, "Periyapatna" },
                    { 16, 3, true, "T. Narasipura" },
                    { 17, 4, true, "Chiknayakanhalli" },
                    { 18, 4, true, "Gubbi" },
                    { 19, 4, true, "Koratagere" },
                    { 20, 4, true, "Kunigal" },
                    { 21, 4, true, "Madhugiri" },
                    { 22, 4, true, "Pavagada" },
                    { 23, 4, true, "Sira" },
                    { 24, 4, true, "Tiptur" },
                    { 25, 4, true, "Tumakuru" },
                    { 26, 4, true, "Turuvekere" },
                    { 27, 5, true, "Bhadravati" },
                    { 28, 5, true, "Hosanagara" },
                    { 29, 5, true, "Kelur" },
                    { 30, 5, true, "Sagara" },
                    { 31, 5, true, "Shikaripur" },
                    { 32, 5, true, "Shivamogga" },
                    { 33, 5, true, "Sorab" },
                    { 34, 5, true, "Thirthahalli" },
                    { 35, 6, true, "Bantwal" },
                    { 36, 6, true, "Belthangady" },
                    { 37, 6, true, "Kadaba" },
                    { 38, 6, true, "Mangaluru" },
                    { 39, 6, true, "Moodabidri" },
                    { 40, 6, true, "Puttur" },
                    { 41, 6, true, "Sullia" },
                    { 42, 6, true, "Vitla" },
                    { 43, 7, true, "Ankola" },
                    { 44, 7, true, "Bhatkal" },
                    { 45, 7, true, "Dandeli" },
                    { 46, 7, true, "Haliyal" },
                    { 47, 7, true, "Honavar" },
                    { 48, 7, true, "Joida" },
                    { 49, 7, true, "Karwar" },
                    { 50, 7, true, "Kumta" },
                    { 51, 7, true, "Mundgod" },
                    { 52, 7, true, "Siddapur" },
                    { 53, 7, true, "Sirsi" },
                    { 54, 7, true, "Yellapur" },
                    { 55, 8, true, "Madikeri" },
                    { 56, 8, true, "Somwarpet" },
                    { 57, 8, true, "Virajpet" },
                    { 58, 9, true, "Alur" },
                    { 59, 9, true, "Arakalagudu" },
                    { 60, 9, true, "Arkalgud" },
                    { 61, 9, true, "Belur" },
                    { 62, 9, true, "Channarayapatna" },
                    { 63, 9, true, "Hassan" },
                    { 64, 9, true, "Holenarasipur" },
                    { 65, 9, true, "Sakaleshapura" },
                    { 66, 10, true, "Chikkamagaluru" },
                    { 67, 10, true, "Kadur" },
                    { 68, 10, true, "Koppa" },
                    { 69, 10, true, "Mudigere" },
                    { 70, 10, true, "N.R. Pura" },
                    { 71, 10, true, "Sringeri" },
                    { 72, 10, true, "Tarikere" },
                    { 73, 11, true, "Kirugavalu" },
                    { 74, 11, true, "Krishnarajapete" },
                    { 75, 11, true, "Maddur" },
                    { 76, 11, true, "Malavalli" },
                    { 77, 11, true, "Mandya" },
                    { 78, 11, true, "Nagamangala" },
                    { 79, 11, true, "Pandavapura" },
                    { 80, 11, true, "Shrirangapattana" },
                    { 81, 12, true, "Chamarajanagar" },
                    { 82, 12, true, "Gundlupet" },
                    { 83, 12, true, "Kollegal" },
                    { 84, 12, true, "Yelandur" },
                    { 85, 13, true, "Channapatna" },
                    { 86, 13, true, "Kanakapura" },
                    { 87, 13, true, "Magadi" },
                    { 88, 13, true, "Ramanagara" },
                    { 89, 14, true, "Bangarpet" },
                    { 90, 14, true, "K.G.F" },
                    { 91, 14, true, "Kolar" },
                    { 92, 14, true, "Malur" },
                    { 93, 14, true, "Mulbagal" },
                    { 94, 14, true, "Srinivaspur" },
                    { 95, 15, true, "Challakere" },
                    { 96, 15, true, "Chitradurga" },
                    { 97, 15, true, "Hiriyur" },
                    { 98, 15, true, "Holalkere" },
                    { 99, 15, true, "Hosadurga" },
                    { 100, 15, true, "Molakalmuru" },
                    { 101, 16, true, "Channagiri" },
                    { 102, 16, true, "Davanagere" },
                    { 103, 16, true, "Harihara" },
                    { 104, 16, true, "Honnali" },
                    { 105, 16, true, "Jagalur" },
                    { 106, 16, true, "Nyamathi" },
                    { 107, 17, true, "Byadagi" },
                    { 108, 17, true, "Hanagal" },
                    { 109, 17, true, "Haveri" },
                    { 110, 17, true, "Hirekerur" },
                    { 111, 17, true, "Ranibennur" },
                    { 112, 17, true, "Savanur" },
                    { 113, 17, true, "Shiggaon" },
                    { 114, 18, true, "Dharwad" },
                    { 115, 18, true, "Hubli" },
                    { 116, 18, true, "Kalghatgi" },
                    { 117, 18, true, "Kundgol" },
                    { 118, 18, true, "Navalgund" },
                    { 119, 19, true, "Gadag" },
                    { 120, 19, true, "Mundargi" },
                    { 121, 19, true, "Nargund" },
                    { 122, 19, true, "Ron" },
                    { 123, 19, true, "Shirahatti" },
                    { 124, 20, true, "Athani" },
                    { 125, 20, true, "Bailhongal" },
                    { 126, 20, true, "Belagavi" },
                    { 127, 20, true, "Chikodi" },
                    { 128, 20, true, "Gokak" },
                    { 129, 20, true, "Hukkeri" },
                    { 130, 20, true, "Kagwad" },
                    { 131, 20, true, "Khanapur" },
                    { 132, 20, true, "Mudalgi" },
                    { 133, 20, true, "Ramdurg" },
                    { 134, 20, true, "Raybag" },
                    { 135, 20, true, "Savadatti" },
                    { 136, 21, true, "Basavana Bagewadi" },
                    { 137, 21, true, "Devar Hippargi" },
                    { 138, 21, true, "Indi" },
                    { 139, 21, true, "Muddebihal" },
                    { 140, 21, true, "Sindagi" },
                    { 141, 21, true, "Vijayapura" },
                    { 142, 22, true, "Bagalkot" },
                    { 143, 22, true, "Badami" },
                    { 144, 22, true, "Bilgi" },
                    { 145, 22, true, "Hunagund" },
                    { 146, 22, true, "Jamkhandi" },
                    { 147, 22, true, "Mudhol" },
                    { 148, 23, true, "Ballari" },
                    { 149, 23, true, "Hadagalli" },
                    { 150, 23, true, "Hagaribommanahalli" },
                    { 151, 23, true, "Hospet" },
                    { 152, 23, true, "Kudligi" },
                    { 153, 23, true, "Sandur" },
                    { 154, 23, true, "Siruguppa" },
                    { 155, 24, true, "Gangavathi" },
                    { 156, 24, true, "Koppal" },
                    { 157, 24, true, "Kustagi" },
                    { 158, 24, true, "Yelburga" },
                    { 159, 25, true, "Devadurga" },
                    { 160, 25, true, "Lingsugur" },
                    { 161, 25, true, "Manvi" },
                    { 162, 25, true, "Raichur" },
                    { 163, 25, true, "Sindhanur" },
                    { 164, 26, true, "Gurmitkal" },
                    { 165, 26, true, "Hunsagi" },
                    { 166, 26, true, "Shahapur" },
                    { 167, 26, true, "Shorapur" },
                    { 168, 26, true, "Yadgir" },
                    { 169, 27, true, "Aland" },
                    { 170, 27, true, "Afzalpur" },
                    { 171, 27, true, "Chincholi" },
                    { 172, 27, true, "Chittapur" },
                    { 173, 27, true, "Jevargi" },
                    { 174, 27, true, "Kalaburagi" },
                    { 175, 27, true, "Sedam" },
                    { 176, 27, true, "Shahaabad" },
                    { 177, 27, true, "Wadi" },
                    { 178, 28, true, "Aurad" },
                    { 179, 28, true, "Basavakalyan" },
                    { 180, 28, true, "Bhalki" },
                    { 181, 28, true, "Bidar" },
                    { 182, 28, true, "Humnabad" },
                    { 183, 29, true, "Karkala" },
                    { 184, 29, true, "Kundapur" },
                    { 185, 29, true, "Udupi" },
                    { 186, 30, true, "Bagepalli" },
                    { 187, 30, true, "Chikkaballapur" },
                    { 188, 30, true, "Chintamani" },
                    { 189, 30, true, "Gauribidanur" },
                    { 190, 30, true, "Gudibanda" },
                    { 191, 30, true, "Sidlaghatta" },
                    { 192, 31, true, "Hagaribommanahalli" },
                    { 193, 31, true, "Hoovina Hadagali" },
                    { 194, 31, true, "Hosapete" },
                    { 195, 31, true, "Kottur" },
                    { 196, 31, true, "Kudligi" },
                    { 197, 31, true, "Sandur" },
                    { 198, 31, true, "Siruguppa" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "Taluks",
                keyColumn: "TalukId",
                keyValue: 198);
        }
    }
}
