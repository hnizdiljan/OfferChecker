          # init variables
          NEW_VERSION=""
          
          # check if there are any tags
          if $(git rev-parse --verify --quiet refs/tags); then
            # get latest release tag
            LATEST_RELEASE_TAG=$(git describe --tags --abbrev=0)
            # increment version number
            NEW_VERSION=$(awk -F. -v OFS=. '{ if (NF == 1) { $NF++; } else { if (length($NF+1) > length($NF)) $(NF-1)++; $NF=sprintf("%0*d", length($NF), ($NF+1)%(10^length($NF))); } print }' <<< "$LATEST_RELEASE_TAG")
            # get list of commits since last release
            COMMITS_SINCE_LAST_RELEASE=$(git log --oneline $LATEST_RELEASE_TAG..HEAD)
            # create release description
            RELEASE_DESCRIPTION="## Changes since last release:\n\n$COMMITS_SINCE_LAST_RELEASE"
            
          fi
          
          echo "Description: ${RELEASE_DESCRIPTION}"
          echo "New version: ${NEW_VERSION}"
          echo "Latest tag: ${LATEST_RELEASE_TAG}"
          
          if [ -z "${NEW_VERSION}" ]
          then
            # set initial version number
            NEW_VERSION="1.0.0"
            # set default release description
            RELEASE_DESCRIPTION="This is the first release of our application"
          fi