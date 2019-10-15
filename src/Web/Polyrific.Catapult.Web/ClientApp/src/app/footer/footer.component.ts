import { Component, OnInit, Input } from '@angular/core';
import { environment } from 'environments/environment';
import { Config, ConfigService } from '@app/config/config.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {
    @Input() showVersionPage: boolean;
    version: string = environment.version;
    environmentName: string = 'Unknown';

    constructor(
        private configService: ConfigService
    ) { }

    ngOnInit() {
        this.environmentName = this.configService.getConfig().environmentName;
    }

}
