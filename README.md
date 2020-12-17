# LauncherSRHD
MyLauncherRepo
Для замены ссылок на сайт, в основном классе (mainform.form) есть глобальные переменные 

		public string browserUrlRu = "https://store.steampowered.com/app/214730/Space_Rangers_HD_A_War_Apart/";
		public string browserUrlEng = "https://store.steampowered.com/app/214730/Space_Rangers_HD_A_War_Apart/";
    
Замена ссылок в них приведет к замене активного сайта. 

Все ссылки на скачивание любых объектов лаунчерами что учавствуют в логике находятся в классе AppInfo.cs, однако я лично без понятия по какой причине ссылки не работают на скачивание. Либо я накосячил что-то исправляя или патча тогда, либо же хостинги что мне я тестировал работали с подвохом. До этого все работало используя ссылки google.drive.api.v3, но не мал шанс что они что-то принципиальное поменяли. 
https://www.wonderplugin.com/online-tools/google-drive-direct-link-generator/#getsharedurl
С помощью этой утилиты я создавал прямые ссылки на google drive. 
