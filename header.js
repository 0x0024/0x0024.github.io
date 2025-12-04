(function() {
   // 현재 파일명+확장자 얻기
   const thisfilefullname = document.URL.substring(document.URL.lastIndexOf('/') + 1, document.URL.length);
   // 현재 파일명 얻기
   const thisfilename = thisfilefullname.substring(thisfilefullname.lastIndexOf('.'), 0);
   document.title = decodeURI(thisfilename);
})();
