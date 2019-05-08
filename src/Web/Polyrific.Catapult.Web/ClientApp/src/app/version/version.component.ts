import { Component, OnInit } from '@angular/core';
import { VersionService } from '@app/core/services/version.service';
import { VersionDto } from '@app/core/models/version/version-dto';
import { environment } from '@env/environment';

@Component({
  selector: 'app-version',
  templateUrl: './version.component.html',
  styleUrls: ['./version.component.css']
})
export class VersionComponent implements OnInit {
  version: VersionDto;
  loading: boolean;
  webVersion: string;

  constructor(private versionService: VersionService) { }

  ngOnInit() {
    this.loading = true;
    this.versionService.getVersions().subscribe((data) => {
      this.version = data;
      this.loading = false;
    });
    this.webVersion = environment.version;
  }

}
