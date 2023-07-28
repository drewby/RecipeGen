FROM mcr.microsoft.com/devcontainers/dotnet:0-7.0

ARG USERNAME=vscode
USER $USERNAME

RUN dotnet dev-certs https -v
RUN dotnet tool install -g dotnet-reportgenerator-globaltool

RUN sh -c "$(wget -O- https://raw.githubusercontent.com/robbyrussell/oh-my-zsh/master/tools/install.sh)" "" --unattended &> /dev/null
ENV ENV="/home/$USERNAME/.ashrc" \
  ZSH=/home/$USERNAME/.oh-my-zsh \
  EDITOR=vi \
  LANG=en_US.UTF-8
RUN printf 'ZSH_THEME="robbyrussell"\nENABLE_CORRECTION="false"\nplugins=(git copyfile extract colorize dotenv encode64 golang)\nsource $ZSH/oh-my-zsh.sh\nexport PATH=$PATH:~/.local/bin' > "/home/$USERNAME/.zshrc"
RUN echo "exec `which zsh`" > "/home/$USERNAME/.ashrc"

ENV SHELL /bin/zsh
