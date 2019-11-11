import { Component, OnInit, Input } from '@angular/core';
import { StickerResultModel } from '../../models/stickers/sticker-result-model';

@Component({
  selector: 'app-sticker',
  templateUrl: './sticker.component.html',
  styleUrls: ['./sticker.component.css']
})
export class StickerComponent implements OnInit {

  @Input() item: StickerResultModel;
  @Input() isSelected: boolean;

  constructor() { }

  ngOnInit() {
  }

  /* dragElement(document.getElementById("sticker"));
 
 function dragElement(elmnt) {
   var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
   if (document.getElementById(elmnt.id + "header")) {
     // if present, the header is where you move the DIV from:
     document.getElementById(elmnt.id + "header").onmousedown = dragMouseDown;
   } else {
     // otherwise, move the DIV from anywhere inside the DIV:
     elmnt.onmousedown = dragMouseDown;
   }
 
   function dragMouseDown(e) {
     e = e || window.event;
     e.preventDefault();
     // get the mouse cursor position at startup:
     pos3 = e.clientX;
     pos4 = e.clientY;
     document.onmouseup = closeDragElement;
     // call a function whenever the cursor moves:
     document.onmousemove = elementDrag;
   }
 
   function elementDrag(e) {
     e = e || window.event;
     e.preventDefault();
     // calculate the new cursor position:
     pos1 = pos3 - e.clientX;
     pos2 = pos4 - e.clientY;
     pos3 = e.clientX;
     pos4 = e.clientY;
     // set the element's new position:
     elmnt.style.top = (elmnt.offsetTop - pos2) + "px";
     elmnt.style.left = (elmnt.offsetLeft - pos1) + "px";
   }
 
   function closeDragElement() {
     // 
     document.onmouseup = null;
     document.onmousemove = null;
   }
 }*/

}
