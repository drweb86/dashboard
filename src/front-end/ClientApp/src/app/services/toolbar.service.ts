import { Injectable } from "@angular/core";
import { MenuItem } from '../models/view/menu-item';

@Injectable()
export class ToolbarServie {
    componentButtons: MenuItem[] = [];
}
