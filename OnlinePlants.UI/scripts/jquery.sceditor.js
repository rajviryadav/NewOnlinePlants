/*----------------------------------------------------*/
/*  SCEditor
/*----------------------------------------------------*/

(function($){
	$(document).ready(function(){
		

		function addIng() {
			var newElem = $('tr.ingredients-cont.ing:first').clone();
			newElem.find('input').val('');
			newElem.appendTo('table#ingredients-sort');
		}

	});
})(this.jQuery);