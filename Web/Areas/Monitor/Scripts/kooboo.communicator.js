(function($){
    $(function(){
        $('.collapse-list li a').click(function(e){
            e.preventDefault();
        });

        $('.chart-btn').toggle(function(e){
            $(this).siblings('.chart-content').animate({'height': 'toggle'}, 300);
            $(this).addClass('active');
            e.preventDefault();
        }, function(e){
            $(this).siblings('.chart-content').animate({'height': 'toggle'}, 300);
            $(this).removeClass('active');
            e.preventDefault();
        });
		
		// Filter
        $('.filter-btn').toggle(function(e){
            $(this).addClass('active');
            $(this).find('img').attr('class', 'icon arrow small gray-up');
            $(this).siblings('.filter-content').animate({'height': 'toggle'}, 200);
            e.preventDefault();
        }, function(e){
            $(this).removeClass('active');
            $(this).find('img').attr('class', 'icon arrow small gray-down');
            $(this).siblings('.filter-content').animate({'height': 'toggle'}, 200);
            e.preventDefault();
        });
		
		// popup
		$('.popup-btn').click(function(){
			$('.ui-widget-overlay, .ui-dialog').show();
		});
		$('.popup-inner .footer .buttons .button.cancel, .ui-widget-overlay').click(function(){
			$('.ui-widget-overlay, .ui-dialog').hide();
		});
    });
	
	// Subscribers-list-create-signup.html
	$(function(){
		var snips = {
			'email': {
				'table': '<tr><td><label for="EmailAddress">Email address<span class="required">*</span></label></td><td><input id="EmailAddress" name="EmailAddress" type="text" value="" /></td></tr>\n',
				'css': '<label for="EmailAddress">Email address<span class="required">*</span></label><input id="EmailAddress" name="EmailAddress" type="text" value="" />\n'
			},
			'firstname': {
				'table': '<tr><td><label for="FirstName">First Name</label></td><td><input id="FirstName" name="FirstName" type="text" value="" /></td></tr>\n',
				'css': '<label for="FirstName">First Name</label><input id="FirstName" name="FirstName" type="text" value="" />\n'
			},
			'lastname': {
				'table': '<tr><td><label for="LastName">Last Name</label></td><td><input id="LastName" name="LastName" type="text" value="" /></td></tr>\n',
				'css': '<label for="LastName">Last Name</label><input id="LastName" name="LastName" type="text" value="" />\n'
			},
			'gender': {
				'table': '<tr><td><label for="gender">Gender</label></td><td><select id="gender" name="gender" ><option value="Male">Male</option><option value="Female">Female</option></select></td></tr>\n',
				'css': '<label for="gender">Gender</label><select id="gender" name="gender" ><option value="Male">Male</option><option value="Female">Female</option></select>\n'
			},
			'form': {
				'start': '<form action="http://app.kooboomail.com/o/signup/vr-eej7eu0-LRgtDOMNRfQ/_QHjL_3jgE6CM8qzdjIaAQ" method="post" accept-charset="utf-8">\n',
				'end': '<button type="submit">Submit</button>\n</form>'
			},
			'table': {
				'start': '<table>\n',
				'end':	'</table>'
			}
		}
		
		$('#firstname, #lastname, #gender, input[name="htmltype"]').change(function(){
			var type = $('input[name="htmltype"]:checked').val();
			HTMLcode = snips.form['start'];
			HTMLcodeEnd = snips.form['end'];
			
			if(type == 'table'){
				HTMLcode += snips.table['start'];
				HTMLcodeEnd = snips.table['end'] + snips.form['end'];
			}
			
			if($('#email').is(':checked')){
				HTMLcode += snips.email[type];
			}
			if($('#firstname').is(':checked')){
				HTMLcode += snips.firstname[type];
			}
			if($('#lastname').is(':checked')){
				HTMLcode += snips.lastname[type];
			}
			if($('#gender').is(':checked')){
				HTMLcode += snips.gender[type];
			}
			
			HTMLcode += HTMLcodeEnd;
			$('#code').html(HTMLcode);
		});
		
		$('#code').html(snips.form['start']+snips.email['css']+snips.form['end']);
	});
})(jQuery);








