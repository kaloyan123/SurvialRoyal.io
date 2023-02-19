const btn = document.getElementById('btnValue');

btn.addEventListener('click', GetValue);

let value_ = 0;
function GetValue()
{

    value_ = document.getElementById('getMyValue').value;
    console.log(value_ );
}